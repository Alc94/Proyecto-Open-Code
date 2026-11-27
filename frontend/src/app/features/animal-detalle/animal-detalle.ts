import { DatePipe, DecimalPipe } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { Component, computed, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ZooApi } from '../../core/api/zoo-api';
import { esNoEncontrado, etiqueta, inicial, textoError } from '../../core/formato';
import {
  AnimalDetalle as FichaAnimal,
  HistorialAnimal,
  Tratamiento,
} from '../../core/models/zoo.model';
import { ErrorCarga } from '../../shared/components/error-carga/error-carga';
import { Esqueleto } from '../../shared/components/esqueleto/esqueleto';
import { EstadoEtiqueta } from '../../shared/components/estado-etiqueta/estado-etiqueta';
import { EstadoVacio } from '../../shared/components/estado-vacio/estado-vacio';
import { Icono } from '../../shared/components/icono/icono';

@Component({
  selector: 'app-animal-detalle',
  imports: [DatePipe, DecimalPipe, RouterLink, ErrorCarga, Esqueleto, EstadoEtiqueta, EstadoVacio, Icono],
  templateUrl: './animal-detalle.html',
})
export class AnimalDetalle {
  private readonly api = inject(ZooApi);

  /** Identificador que llega por la ruta /animales/:id. */
  readonly id = input.required<string>();

  protected readonly animalId = computed(() => {
    const numero = Number(this.id());
    return Number.isInteger(numero) && numero > 0 ? numero : null;
  });

  protected readonly detalle = httpResource<FichaAnimal>(() => {
    const id = this.animalId();
    return id ? this.api.animal(id) : undefined;
  });

  protected readonly historial = httpResource<HistorialAnimal>(() => {
    const id = this.animalId();
    return id ? this.api.historialAnimal(id) : undefined;
  });

  protected readonly tratamientos = httpResource<Tratamiento[]>(() => {
    const id = this.animalId();
    return id ? this.api.tratamientos({ animalId: id, activo: true }) : undefined;
  });

  protected readonly noEncontrado = computed(() => esNoEncontrado(this.detalle.error()));
  protected readonly errorDetalle = computed(() =>
    this.detalle.error()
      ? textoError(this.detalle.error(), 'No se pudo cargar la ficha del animal.')
      : '',
  );
  protected readonly errorHistorial = computed(() =>
    this.historial.error()
      ? textoError(this.historial.error(), 'No se pudo cargar el historial del animal.')
      : '',
  );
  protected readonly errorTratamientos = computed(() =>
    this.tratamientos.error()
      ? textoError(this.tratamientos.error(), 'No se pudieron cargar los tratamientos.')
      : '',
  );

  protected readonly alertas = computed(() =>
    this.historial.hasValue() ? this.historial.value()!.alertas : [],
  );
  protected readonly alertasAbiertas = computed(
    () => this.alertas().filter((alerta) => alerta.estado === 'Abierta').length,
  );
  protected readonly consultas = computed(() =>
    this.historial.hasValue() ? this.historial.value()!.consultas : [],
  );
  protected readonly alimentacion = computed(() =>
    this.historial.hasValue() ? this.historial.value()!.alimentacion : [],
  );
  protected readonly tratamientosActivos = computed(() =>
    this.tratamientos.hasValue() ? this.tratamientos.value()! : [],
  );

  protected readonly etiqueta = etiqueta;
  protected readonly inicial = inicial;
}
