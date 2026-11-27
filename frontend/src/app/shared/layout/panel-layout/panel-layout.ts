import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Icono, IconoNombre } from '../../components/icono/icono';

interface EnlaceNavegacion {
  ruta: string;
  texto: string;
  icono: IconoNombre;
}

/** Marco de las pantallas internas del zoo: barra lateral y navegación móvil. */
@Component({
  selector: 'app-panel-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, Icono],
  templateUrl: './panel-layout.html',
})
export class PanelLayout {
  protected readonly enlaces: EnlaceNavegacion[] = [
    { ruta: '/', texto: 'Inicio', icono: 'inicio' },
    { ruta: '/panel', texto: 'Panel', icono: 'panel' },
    { ruta: '/animales', texto: 'Animales', icono: 'animales' },
    { ruta: '/alimentacion', texto: 'Alimentación', icono: 'alimentacion' },
  ];

  protected esExacto(ruta: string): boolean {
    return ruta === '/' || ruta === '/panel';
  }
}
