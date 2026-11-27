import { httpResource } from '@angular/common/http';
import { Component, computed, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ZooApi } from '../../core/api/zoo-api';
import { Alerta, ResumenPanel } from '../../core/models/zoo.model';
import { barraGravedad, textoError } from '../../core/formato';
import { ErrorCarga } from '../../shared/components/error-carga/error-carga';
import { Esqueleto } from '../../shared/components/esqueleto/esqueleto';
import { EstadoEtiqueta } from '../../shared/components/estado-etiqueta/estado-etiqueta';
import { EstadoVacio } from '../../shared/components/estado-vacio/estado-vacio';
import { Icono } from '../../shared/components/icono/icono';

type Tono = 'neutro' | 'tratamiento' | 'alerta' | 'grave' | 'dieta';

interface TarjetaResumen {
  etiqueta: string;
  valor: number;
  nota: string;
  tono: Tono;
}

@Component({
  selector: 'app-panel',
  imports: [DatePipe, RouterLink, ErrorCarga, Esqueleto, EstadoEtiqueta, EstadoVacio, Icono],
  templateUrl: './panel.html',
})
export class Panel {
  private readonly api = inject(ZooApi);

  protected readonly fechaHoy = new Date();
  protected readonly resumen = httpResource<ResumenPanel>(() => this.api.resumenPanel());
  protected readonly alertas = httpResource<Alerta[]>(() => this.api.alertas({ estado: 'Abierta' }));

  protected readonly tarjetas = computed<TarjetaResumen[]>(() => {
    if (!this.resumen.hasValue()) {
      return [];
    }

    const r = this.resumen.value()!;

    return [
      { etiqueta: 'Animales', valor: r.totalAnimales, nota: 'en el censo', tono: 'neutro' },
      {
        etiqueta: 'En tratamiento',
        valor: r.animalesEnTratamiento,
        nota: 'con seguimiento activo',
        tono: 'tratamiento',
      },
      {
        etiqueta: 'Alertas abiertas',
        valor: r.alertasAbiertas,
        nota: 'pendientes de revisar',
        tono: 'alerta',
      },
      {
        etiqueta: 'Alertas graves',
        valor: r.alertasGravesAbiertas,
        nota: 'prioridad alta o crítica',
        tono: 'grave',
      },
      {
        etiqueta: 'Dietas propuestas',
        valor: r.dietasEnPropuesta,
        nota: 'pendientes de validar',
        tono: 'dieta',
      },
    ];
  });

  protected readonly mensajeResumen = computed(() => {
    if (this.resumen.error()) {
      return 'No se pudo consultar el estado del zoo.';
    }

    if (!this.resumen.hasValue()) {
      return 'Consultando el estado del zoo.';
    }

    const r = this.resumen.value()!;
    const avisos: string[] = [];

    if (r.alertasGravesAbiertas > 0) {
      avisos.push(
        r.alertasGravesAbiertas === 1
          ? '1 alerta grave requiere revisión'
          : `${r.alertasGravesAbiertas} alertas graves requieren revisión`,
      );
    }

    if (r.animalesEnTratamiento > 0) {
      avisos.push(
        r.animalesEnTratamiento === 1
          ? '1 animal sigue en tratamiento'
          : `${r.animalesEnTratamiento} animales siguen en tratamiento`,
      );
    }

    return avisos.length > 0
      ? `${avisos.join('. ')}.`
      : 'No hay alertas graves ni tratamientos activos en este momento.';
  });

  protected readonly alertasAbiertas = computed(() =>
    this.alertas.hasValue() ? this.alertas.value()! : [],
  );
  protected readonly errorResumen = computed(() =>
    this.resumen.error()
      ? textoError(this.resumen.error(), 'No se pudo cargar el resumen del panel.')
      : '',
  );
  protected readonly errorAlertas = computed(() =>
    this.alertas.error()
      ? textoError(this.alertas.error(), 'No se pudieron cargar las alertas.')
      : '',
  );

  protected readonly barra = barraGravedad;

  protected actualizar(): void {
    this.resumen.reload();
    this.alertas.reload();
  }

  protected claseValor(tono: Tono): string {
    const color = {
      neutro: 'text-bosque-900',
      tratamiento: 'text-ambar-600',
      alerta: 'text-arcilla-600',
      grave: 'text-arcilla-700',
      dieta: 'text-bosque-700',
    }[tono];

    return `mt-2 font-display text-4xl font-semibold tabular-nums ${color}`;
  }
}
