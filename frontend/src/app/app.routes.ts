import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'panel' },
  {
    path: 'panel',
    title: 'Panel · Zoo-Logic',
    loadComponent: () => import('./features/panel/panel').then((m) => m.Panel),
  },
  {
    path: 'animales',
    title: 'Animales · Zoo-Logic',
    loadComponent: () => import('./features/animales/animales').then((m) => m.Animales),
  },
  {
    path: 'animales/:id',
    title: 'Ficha de animal · Zoo-Logic',
    loadComponent: () =>
      import('./features/animal-detalle/animal-detalle').then((m) => m.AnimalDetalle),
  },
  {
    path: 'alimentacion',
    title: 'Alimentación · Zoo-Logic',
    loadComponent: () =>
      import('./features/alimentacion/alimentacion').then((m) => m.Alimentacion),
  },
  { path: '**', redirectTo: 'panel' },
];
