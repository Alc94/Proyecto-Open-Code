import { Injectable } from '@angular/core';

type Parametros = Record<string, string | number | boolean | null | undefined>;

/**
 * Construye las rutas de la API. Todas son relativas y empiezan por /api:
 * en desarrollo las resuelve el proxy de Angular y en producción la propia API.
 */
@Injectable({ providedIn: 'root' })
export class ZooApi {
  resumenPanel(): string {
    return '/api/panel/resumen';
  }

  alertas(filtros: { estado?: string; gravedad?: string; animalId?: number } = {}): string {
    return this.conUrl('/api/alertas', filtros);
  }

  animales(
    filtros: {
      estado?: string;
      especieId?: number;
      recinto?: string;
      texto?: string;
      pagina?: number;
      tamano?: number;
    } = {},
  ): string {
    return this.conUrl('/api/animales', filtros);
  }

  animal(id: number): string {
    return `/api/animales/${id}`;
  }

  historialAnimal(id: number): string {
    return `/api/animales/${id}/historial`;
  }

  tratamientos(filtros: { animalId?: number; activo?: boolean } = {}): string {
    return this.conUrl('/api/tratamientos', filtros);
  }

  alimentos(filtros: { categoria?: string; texto?: string } = {}): string {
    return this.conUrl('/api/alimentos', filtros);
  }

  dietas(filtros: { especieId?: number; estado?: string } = {}): string {
    return this.conUrl('/api/dietas', filtros);
  }

  dieta(id: number): string {
    return `/api/dietas/${id}`;
  }

  private conUrl(base: string, parametros: Parametros): string {
    const busqueda = new URLSearchParams();

    for (const [clave, valor] of Object.entries(parametros)) {
      if (valor === undefined || valor === null || valor === '') {
        continue;
      }

      busqueda.set(clave, String(valor));
    }

    const consulta = busqueda.toString();
    return consulta ? `${base}?${consulta}` : base;
  }
}
