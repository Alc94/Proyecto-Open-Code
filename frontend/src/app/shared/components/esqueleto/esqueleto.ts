import { Component, input } from '@angular/core';

@Component({
  selector: 'app-esqueleto',
  template: `<div class="animate-pulse rounded-md bg-bosque-200/70" [class]="clase()" aria-hidden="true"></div>`,
})
export class Esqueleto {
  readonly clase = input('h-4 w-full');
}
