import { HttpErrorResponse } from '@angular/common/http';

export type TipoEtiqueta = 'animal' | 'alerta' | 'gravedad' | 'dieta';

const ETIQUETAS: Record<string, string> = {
  Activo: 'Activo',
  EnObservacion: 'En observación',
  EnTratamiento: 'En tratamiento',
  Cuarentena: 'Cuarentena',
  Abierta: 'Abierta',
  Revisada: 'Revisada',
  Cerrada: 'Cerrada',
  Baja: 'Baja',
  Media: 'Media',
  Alta: 'Alta',
  Critica: 'Crítica',
  Propuesta: 'Propuesta',
  Validada: 'Validada',
  Descartada: 'Descartada',
  Mamifero: 'Mamífero',
  Ave: 'Ave',
  Reptil: 'Reptil',
  Anfibio: 'Anfibio',
  Pez: 'Pez',
  Carnivoro: 'Carnívoro',
  Herbivoro: 'Herbívoro',
  Omnivoro: 'Omnívoro',
  Macho: 'Macho',
  Hembra: 'Hembra',
};

const BASE_ETIQUETA =
  'inline-flex items-center gap-1.5 rounded-full px-2.5 py-0.5 text-xs font-medium ring-1 ring-inset';

const VARIANTES: Record<TipoEtiqueta, Record<string, string>> = {
  animal: {
    Activo: 'bg-bosque-100 text-bosque-800 ring-bosque-200',
    EnObservacion: 'bg-ambar-100 text-ambar-800 ring-ambar-200',
    EnTratamiento: 'bg-arcilla-100 text-arcilla-800 ring-arcilla-200',
    Cuarentena: 'bg-stone-100 text-stone-700 ring-stone-200',
  },
  alerta: {
    Abierta: 'bg-arcilla-100 text-arcilla-800 ring-arcilla-200',
    Revisada: 'bg-ambar-100 text-ambar-800 ring-ambar-200',
    Cerrada: 'bg-bosque-100 text-bosque-700 ring-bosque-200',
  },
  gravedad: {
    Baja: 'bg-bosque-100 text-bosque-700 ring-bosque-200',
    Media: 'bg-ambar-100 text-ambar-800 ring-ambar-200',
    Alta: 'bg-arcilla-100 text-arcilla-800 ring-arcilla-200',
    Critica: 'bg-red-100 text-red-800 ring-red-200',
  },
  dieta: {
    Propuesta: 'bg-ambar-100 text-ambar-800 ring-ambar-200',
    Validada: 'bg-bosque-100 text-bosque-800 ring-bosque-200',
    Descartada: 'bg-stone-100 text-stone-700 ring-stone-200',
  },
};

export function etiqueta(valor: string): string {
  return ETIQUETAS[valor] ?? valor;
}

export function clasesEtiqueta(tipo: TipoEtiqueta, valor: string): string {
  const variante = VARIANTES[tipo][valor] ?? 'bg-stone-100 text-stone-700 ring-stone-200';
  return `${BASE_ETIQUETA} ${variante}`;
}

export function barraGravedad(gravedad: string): string {
  switch (gravedad) {
    case 'Critica':
      return 'bg-red-700';
    case 'Alta':
      return 'bg-arcilla-500';
    case 'Media':
      return 'bg-ambar-500';
    default:
      return 'bg-bosque-400';
  }
}

export function inicial(nombre: string): string {
  return nombre.trim().charAt(0).toLocaleUpperCase('es');
}

export function sinTildes(texto: string): string {
  return texto
    .normalize('NFD')
    .replace(/\p{Diacritic}/gu, '')
    .toLocaleLowerCase('es');
}

export function textoError(error: unknown, porDefecto: string): string {
  if (error instanceof HttpErrorResponse) {
    if (error.status === 0) {
      return 'No se pudo conectar con la API. Comprueba que el servidor esté en marcha.';
    }

    if (error.status === 404) {
      return 'No encontramos el recurso solicitado.';
    }

    if (error.status >= 500) {
      // El proxy de desarrollo responde 500 con el cuerpo vacío cuando la API no está
      // levantada; un 500 real de la API llega como ProblemDetails en JSON.
      if (esFalloDeConexion(error)) {
        return 'No se pudo conectar con la API. Comprueba que el servidor esté en marcha.';
      }

      return `La API devolvió un error interno (código ${error.status}). Inténtalo de nuevo.`;
    }

    return `La API respondió con el código ${error.status}.`;
  }

  return porDefecto;
}

export function esNoEncontrado(error: unknown): boolean {
  return error instanceof HttpErrorResponse && error.status === 404;
}

/**
 * Detecta fallos del proxy de desarrollo (API apagada) frente a errores reales de la API.
 * El proxy responde 500 con cuerpo vacío o de texto plano; la API responde JSON.
 */
function esFalloDeConexion(error: HttpErrorResponse): boolean {
  const cuerpo = error.error;

  if (cuerpo === null || cuerpo === undefined || cuerpo === '') {
    return true;
  }

  if (typeof cuerpo === 'string') {
    return /proxy|ECONNREFUSED|ECONNRESET|socket hang up|failed to fetch/i.test(cuerpo);
  }

  return false;
}
