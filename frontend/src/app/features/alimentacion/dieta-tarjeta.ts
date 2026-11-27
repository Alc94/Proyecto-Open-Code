import { DecimalPipe } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { ZooApi } from '../../core/api/zoo-api';
import { textoError } from '../../core/formato';
import { Dieta, DietaLista } from '../../core/models/zoo.model';
import { ErrorCarga } from '../../shared/components/error-carga/error-carga';
import { Esqueleto } from '../../shared/components/esqueleto/esqueleto';
import { EstadoEtiqueta } from '../../shared/components/estado-etiqueta/estado-etiqueta';
import { Icono } from '../../shared/components/icono/icono';

@Component({
  selector: 'app-dieta-tarjeta',
  imports: [DecimalPipe, ErrorCarga, Esqueleto, EstadoEtiqueta, Icono],
  templateUrl: './dieta-tarjeta.html',
})
export class DietaTarjeta {
  private readonly api = inject(ZooApi);

  readonly dieta = input.required<DietaLista>();
  readonly abrirInicial = input(false);

  protected readonly expandida = signal(false);

  protected readonly detalle = httpResource<Dieta>(() => {
    if (!this.expandida()) {
      return undefined;
    }

    return this.api.dieta(this.dieta().id);
  });

  protected readonly errorDetalle = computed(() =>
    this.detalle.error()
      ? textoError(this.detalle.error(), 'No se pudieron cargar las líneas de la dieta.')
      : '',
  );

  constructor() {
    effect(() => {
      if (this.abrirInicial()) {
        this.expandida.set(true);
      }
    });
  }

  protected alternar(): void {
    this.expandida.update((abierta) => !abierta);
  }
}
