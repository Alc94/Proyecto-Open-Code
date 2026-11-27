import { Component, computed, input } from '@angular/core';
import { clasesEtiqueta, etiqueta, TipoEtiqueta } from '../../../core/formato';

@Component({
  selector: 'app-estado-etiqueta',
  template: `
    <span [class]="clases()">
      <span class="size-1.5 rounded-full bg-current" aria-hidden="true"></span>
      {{ texto() }}
    </span>
  `,
})
export class EstadoEtiqueta {
  readonly valor = input.required<string>();
  readonly tipo = input<TipoEtiqueta>('animal');

  protected readonly texto = computed(() => etiqueta(this.valor()));
  protected readonly clases = computed(() => clasesEtiqueta(this.tipo(), this.valor()));
}
