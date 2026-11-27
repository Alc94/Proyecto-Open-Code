import { Component, input, output } from '@angular/core';
import { Icono } from '../icono/icono';

@Component({
  selector: 'app-error-carga',
  imports: [Icono],
  template: `
    <div role="alert" class="rounded-lg border border-arcilla-200 bg-arcilla-50 p-5">
      <div class="flex gap-3">
        <app-icono nombre="alerta" clase="size-5 text-arcilla-600" />
        <div class="min-w-0">
          <p class="font-medium text-arcilla-900">{{ titulo() }}</p>
          <p class="mt-1 text-sm leading-relaxed text-arcilla-800">{{ mensaje() }}</p>
          <button type="button" class="mt-3 text-sm font-semibold text-arcilla-800 underline underline-offset-4 hover:text-arcilla-900" (click)="reintentar.emit()">
            Reintentar
          </button>
        </div>
      </div>
    </div>
  `,
})
export class ErrorCarga {
  readonly titulo = input('No se pudieron cargar los datos');
  readonly mensaje = input.required<string>();
  readonly reintentar = output<void>();
}
