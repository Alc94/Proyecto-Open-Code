import { Component, input } from '@angular/core';

export type IconoNombre =
  | 'panel'
  | 'animales'
  | 'alimentacion'
  | 'buscar'
  | 'flecha-derecha'
  | 'flecha-izquierda'
  | 'alerta'
  | 'calendario'
  | 'peso'
  | 'recinto'
  | 'tratamiento'
  | 'mas'
  | 'menos'
  | 'reloj'
  | 'comprobado'
  | 'cerrar'
  | 'comida';

@Component({
  selector: 'app-icono',
  template: `
    <svg
      xmlns="http://www.w3.org/2000/svg"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      stroke-width="1.75"
      stroke-linecap="round"
      stroke-linejoin="round"
      [class]="'block shrink-0 ' + clase()"
      aria-hidden="true"
      focusable="false"
    >
      @switch (nombre()) {
        @case ('panel') {
          <rect x="3" y="3" width="7" height="9" rx="1" />
          <rect x="14" y="3" width="7" height="5" rx="1" />
          <rect x="14" y="12" width="7" height="9" rx="1" />
          <rect x="3" y="16" width="7" height="5" rx="1" />
        }
        @case ('animales') {
          <circle cx="11" cy="4" r="2" />
          <circle cx="18" cy="8" r="2" />
          <circle cx="20" cy="16" r="2" />
          <path d="M9 10a5 5 0 0 1 5 5v3.5a3.5 3.5 0 0 1-6.84 1.045Q6.52 17.48 4.46 16.84A3.5 3.5 0 0 1 5.5 10Z" />
        }
        @case ('alimentacion') {
          <path d="M11 20A7 7 0 0 1 9.8 6.1C15.5 5 17 4.48 19 2c1 2 2 4.18 2 8 0 5.5-4.78 10-10 10Z" />
          <path d="M2 21c0-3 1.85-5.36 5.08-6C9.5 14.52 12 13 13 12" />
        }
        @case ('buscar') {
          <circle cx="11" cy="11" r="8" />
          <path d="m21 21-4.3-4.3" />
        }
        @case ('flecha-derecha') {
          <path d="M5 12h14" />
          <path d="m12 5 7 7-7 7" />
        }
        @case ('flecha-izquierda') {
          <path d="M19 12H5" />
          <path d="m12 19-7-7 7-7" />
        }
        @case ('alerta') {
          <path d="m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3Z" />
          <path d="M12 9v4" />
          <path d="M12 17h.01" />
        }
        @case ('calendario') {
          <rect x="3" y="4" width="18" height="18" rx="2" />
          <path d="M16 2v4M8 2v4M3 10h18" />
        }
        @case ('peso') {
          <path d="m16 16 3-8 3 8c-.87.65-1.92 1-3 1s-2.13-.35-3-1Z" />
          <path d="m2 16 3-8 3 8c-.87.65-1.92 1-3 1s-2.13-.35-3-1Z" />
          <path d="M7 21h10" />
          <path d="M12 3v18" />
          <path d="M3 7h2c2 0 5-1 7-2 2 1 5 2 7 2h2" />
        }
        @case ('recinto') {
          <path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z" />
          <circle cx="12" cy="10" r="3" />
        }
        @case ('tratamiento') {
          <path d="m10.5 20.5 10-10a4.95 4.95 0 1 0-7-7l-10 10a4.95 4.95 0 1 0 7 7Z" />
          <path d="m8.5 8.5 7 7" />
        }
        @case ('mas') {
          <path d="M12 5v14M5 12h14" />
        }
        @case ('menos') {
          <path d="M5 12h14" />
        }
        @case ('reloj') {
          <circle cx="12" cy="12" r="10" />
          <path d="M12 6v6l4 2" />
        }
        @case ('comprobado') {
          <path d="M20 6 9 17l-5-5" />
        }
        @case ('cerrar') {
          <path d="M18 6 6 18M6 6l12 12" />
        }
        @case ('comida') {
          <path d="M3 2v7c0 1.1.9 2 2 2h4a2 2 0 0 0 2-2V2" />
          <path d="M7 2v20" />
          <path d="M21 15V2a5 5 0 0 0-5 5v6c0 1.1.9 2 2 2h3Zm0 0v7" />
        }
      }
    </svg>
  `,
})
export class Icono {
  readonly nombre = input.required<IconoNombre>();
  readonly clase = input('size-5');
}
