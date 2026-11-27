import { DecimalPipe } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { Component, computed, inject, input, linkedSignal, signal } from '@angular/core';
import { ZooApi } from '../../core/api/zoo-api';
import { sinTildes, textoError } from '../../core/formato';
import { Alimento, DietaLista } from '../../core/models/zoo.model';
import { ErrorCarga } from '../../shared/components/error-carga/error-carga';
import { Esqueleto } from '../../shared/components/esqueleto/esqueleto';
import { EstadoVacio } from '../../shared/components/estado-vacio/estado-vacio';
import { Icono } from '../../shared/components/icono/icono';
import { DietaTarjeta } from './dieta-tarjeta';

type Pestana = 'dietas' | 'alimentos';

@Component({
  selector: 'app-alimentacion',
  imports: [DecimalPipe, ErrorCarga, Esqueleto, EstadoVacio, Icono, DietaTarjeta],
  templateUrl: './alimentacion.html',
})
export class Alimentacion {
  private readonly api = inject(ZooApi);

  /** Filtros que llegan por la URL desde el panel o la ficha de un animal. */
  readonly estado = input<string>('');
  readonly dieta = input<string>('');

  protected readonly pestana = signal<Pestana>('dietas');
  protected readonly estadoFiltro = linkedSignal(() => this.estado() ?? '');

  protected readonly dietas = httpResource<DietaLista[]>(() =>
    this.api.dietas({ estado: this.estadoFiltro() }),
  );
  protected readonly alimentos = httpResource<Alimento[]>(() => this.api.alimentos());

  protected readonly textoAlimento = signal('');
  protected readonly categoria = signal('');

  protected readonly estadosDieta = [
    { valor: '', texto: 'Todos los estados' },
    { valor: 'Propuesta', texto: 'Propuesta' },
    { valor: 'Validada', texto: 'Validada' },
    { valor: 'Descartada', texto: 'Descartada' },
  ];

  protected readonly categorias = computed(() => {
    if (!this.alimentos.hasValue()) {
      return [];
    }

    return [...new Set(this.alimentos.value()!.map((alimento) => alimento.categoria))].sort((a, b) =>
      a.localeCompare(b, 'es'),
    );
  });

  protected readonly alimentosFiltrados = computed(() => {
    if (!this.alimentos.hasValue()) {
      return [];
    }

    const texto = sinTildes(this.textoAlimento().trim());
    const categoria = this.categoria();

    return this.alimentos.value()!.filter((alimento) => {
      const coincideCategoria = categoria === '' || alimento.categoria === categoria;
      const coincideTexto =
        texto === '' ||
        sinTildes(alimento.nombre).includes(texto) ||
        sinTildes(alimento.categoria).includes(texto);

      return coincideCategoria && coincideTexto;
    });
  });

  protected readonly dietaSeleccionada = computed(() => {
    const numero = Number(this.dieta());
    return Number.isInteger(numero) && numero > 0 ? numero : null;
  });

  protected readonly propuestas = computed(() => {
    if (!this.dietas.hasValue()) {
      return 0;
    }

    return this.dietas.value()!.filter((dieta) => dieta.estado === 'Propuesta').length;
  });

  protected readonly hayFiltrosDietas = computed(() => this.estadoFiltro() !== '');
  protected readonly hayFiltrosAlimentos = computed(
    () => this.textoAlimento() !== '' || this.categoria() !== '',
  );

  protected readonly errorDietas = computed(() =>
    this.dietas.error()
      ? textoError(this.dietas.error(), 'No se pudieron cargar las dietas.')
      : '',
  );
  protected readonly errorAlimentos = computed(() =>
    this.alimentos.error()
      ? textoError(this.alimentos.error(), 'No se pudo cargar el catálogo de alimentos.')
      : '',
  );

  protected cambiarEstado(valor: string): void {
    this.estadoFiltro.set(valor);
  }

  protected limpiarDietas(): void {
    this.estadoFiltro.set('');
  }

  protected limpiarAlimentos(): void {
    this.textoAlimento.set('');
    this.categoria.set('');
  }

  protected clasePestana(pestana: Pestana): string {
    const base = 'inline-flex items-center gap-2 rounded-md px-3.5 py-2 text-sm font-medium transition-colors';
    const activa =
      pestana === this.pestana()
        ? 'bg-bosque-800 text-white'
        : 'text-bosque-700 hover:bg-bosque-100';

    return `${base} ${activa}`;
  }

  protected claseContador(pestana: Pestana): string {
    return pestana === this.pestana()
      ? 'rounded-full bg-white/15 px-2 py-0.5 text-xs text-white'
      : 'rounded-full bg-bosque-100 px-2 py-0.5 text-xs text-bosque-700';
  }
}
