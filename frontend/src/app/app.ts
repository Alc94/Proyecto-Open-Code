import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Icono, IconoNombre } from './shared/components/icono/icono';

interface EnlaceNavegacion {
  ruta: string;
  texto: string;
  icono: IconoNombre;
}

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, Icono],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly enlaces: EnlaceNavegacion[] = [
    { ruta: '/panel', texto: 'Panel', icono: 'panel' },
    { ruta: '/animales', texto: 'Animales', icono: 'animales' },
    { ruta: '/alimentacion', texto: 'Alimentación', icono: 'alimentacion' },
  ];
}
