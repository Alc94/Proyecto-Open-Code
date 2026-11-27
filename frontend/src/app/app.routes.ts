import { Routes } from '@angular/router';
import { PanelLayout } from './shared/layout/panel-layout/panel-layout';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    title: 'Zoo-Logic · Zoológico de conservación',
    loadComponent: () => import('./features/inicio/inicio').then((m) => m.Inicio),
  },
  {
    path: '',
    component: PanelLayout,
    children: [
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
    ],
  },
  { path: '**', redirectTo: '' },
];
