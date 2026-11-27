import { httpResource } from '@angular/common/http';
import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ZooApi } from '../../core/api/zoo-api';
import { clasesConservacion, etiqueta, textoError } from '../../core/formato';
import { Especie, ResumenPanel } from '../../core/models/zoo.model';
import { ErrorCarga } from '../../shared/components/error-carga/error-carga';
import { Esqueleto } from '../../shared/components/esqueleto/esqueleto';
import { EstadoVacio } from '../../shared/components/estado-vacio/estado-vacio';
import { Icono } from '../../shared/components/icono/icono';

/** Portada pública del zoológico; el acceso al panel queda como zona del personal. */
@Component({
  selector: 'app-inicio',
  imports: [RouterLink, ErrorCarga, Esqueleto, EstadoVacio, Icono],
  templateUrl: './inicio.html',
})
export class Inicio {
  private readonly api = inject(ZooApi);

  protected readonly resumen = httpResource<ResumenPanel>(() => this.api.resumenPanel());
  protected readonly especies = httpResource<Especie[]>(() => this.api.especies());

  protected readonly totalEspecies = computed(() =>
    this.especies.hasValue() ? this.especies.value()!.length : null,
  );

  protected readonly errorResumen = computed(() =>
    this.resumen.error()
      ? textoError(this.resumen.error(), 'No se pudieron cargar las cifras del zoo.')
      : '',
  );
  protected readonly errorEspecies = computed(() =>
    this.especies.error()
      ? textoError(this.especies.error(), 'No se pudieron cargar las especies.')
      : '',
  );

  protected readonly etiqueta = etiqueta;
  protected readonly clasesConservacion = clasesConservacion;

  protected reintentarCifras(): void {
    this.resumen.reload();
    this.especies.reload();
  }
}
