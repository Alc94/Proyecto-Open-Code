import { DecimalPipe } from '@angular/common';
import { httpResource } from '@angular/common/http';
import { Component, computed, inject, input, linkedSignal, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { ZooApi } from '../../core/api/zoo-api';
import { etiqueta, textoError } from '../../core/formato';
import { AnimalLista, Pagina } from '../../core/models/zoo.model';
import { ErrorCarga } from '../../shared/components/error-carga/error-carga';
import { Esqueleto } from '../../shared/components/esqueleto/esqueleto';
import { EstadoEtiqueta } from '../../shared/components/estado-etiqueta/estado-etiqueta';
import { EstadoVacio } from '../../shared/components/estado-vacio/estado-vacio';
import { Icono } from '../../shared/components/icono/icono';

@Component({
  selector: 'app-animales',
  imports: [
    DecimalPipe,
    RouterLink,
    ErrorCarga,
    Esqueleto,
    EstadoEtiqueta,
    EstadoVacio,
    Icono,
  ],
  templateUrl: './animales.html',
})
export class Animales {
  private readonly api = inject(ZooApi);
  private readonly busqueda$ = new Subject<string>();

  /** Filtros iniciales que llegan por la URL, por ejemplo /animales?estado=EnTratamiento. */
  readonly estado = input<string>('');
  readonly texto = input<string>('');

  protected readonly estadoFiltro = linkedSignal(() => this.estado() ?? '');
  protected readonly textoEntrada = linkedSignal(() => this.texto() ?? '');
  protected readonly textoAplicado = linkedSignal(() => this.texto() ?? '');
  protected readonly paginaActual = signal(1);
  protected readonly tamanoPagina = 12;

  protected readonly estados = [
    { valor: '', texto: 'Todos los estados' },
    { valor: 'Activo', texto: 'Activo' },
    { valor: 'EnObservacion', texto: 'En observación' },
    { valor: 'EnTratamiento', texto: 'En tratamiento' },
    { valor: 'Cuarentena', texto: 'Cuarentena' },
  ];

  protected readonly animales = httpResource<Pagina<AnimalLista>>(() =>
    this.api.animales({
      estado: this.estadoFiltro(),
      texto: this.textoAplicado(),
      pagina: this.paginaActual(),
      tamano: this.tamanoPagina,
    }),
  );

  protected readonly totalPaginas = computed(() => {
    if (!this.animales.hasValue()) {
      return 1;
    }

    const resultado = this.animales.value()!;
    return Math.max(1, Math.ceil(resultado.total / resultado.tamanoPagina));
  });

  protected readonly rango = computed(() => {
    if (!this.animales.hasValue()) {
      return { desde: 0, hasta: 0 };
    }

    const resultado = this.animales.value()!;

    if (resultado.total === 0) {
      return { desde: 0, hasta: 0 };
    }

    const desde = (resultado.pagina - 1) * resultado.tamanoPagina + 1;
    const hasta = Math.min(resultado.pagina * resultado.tamanoPagina, resultado.total);
    return { desde, hasta };
  });

  protected readonly hayFiltros = computed(
    () => this.estadoFiltro() !== '' || this.textoEntrada() !== '',
  );

  protected readonly error = computed(() =>
    this.animales.error()
      ? textoError(this.animales.error(), 'No se pudo cargar el listado de animales.')
      : '',
  );

  protected readonly etiqueta = etiqueta;

  constructor() {
    this.busqueda$
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntilDestroyed())
      .subscribe((valor) => {
        this.textoAplicado.set(valor);
        this.paginaActual.set(1);
      });
  }

  protected alEscribir(valor: string): void {
    this.textoEntrada.set(valor);
    this.busqueda$.next(valor);
  }

  protected cambiarEstado(valor: string): void {
    this.estadoFiltro.set(valor);
    this.paginaActual.set(1);
  }

  protected limpiar(): void {
    this.estadoFiltro.set('');
    this.textoEntrada.set('');
    this.textoAplicado.set('');
    this.paginaActual.set(1);
    this.busqueda$.next('');
  }

  protected cambiarPagina(destino: number): void {
    this.paginaActual.set(Math.min(Math.max(1, destino), this.totalPaginas()));
  }

  protected claseAlertas(animal: AnimalLista): string {
    return (
      'inline-flex items-center gap-1 text-xs font-medium ' +
      (animal.tieneAlertaGrave ? 'text-arcilla-700' : 'text-ambar-600')
    );
  }
}
