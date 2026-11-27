import { Component, input } from '@angular/core';
import { Icono, IconoNombre } from '../icono/icono';

@Component({
  selector: 'app-estado-vacio',
  imports: [Icono],
  template: `
    <div class="flex flex-col items-center justify-center rounded-lg border border-dashed border-bosque-300 bg-arena-50/70 px-6 py-12 text-center">
      <span class="grid size-12 place-items-center rounded-full bg-bosque-100 text-bosque-600">
        <app-icono [nombre]="icono()" clase="size-6" />
      </span>
      <p class="mt-4 font-display text-xl font-semibold text-bosque-950">{{ titulo() }}</p>
      @if (mensaje()) {
        <p class="mt-1 max-w-sm text-sm leading-relaxed text-bosque-600">{{ mensaje() }}</p>
      }
      <div class="mt-4">
        <ng-content />
      </div>
    </div>
  `,
})
export class EstadoVacio {
  readonly titulo = input.required<string>();
  readonly mensaje = input('');
  readonly icono = input<IconoNombre>('animales');
}
