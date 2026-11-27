/** Contratos que devuelve la API de Zoo-Logic. */

export interface Pagina<T> {
  elementos: T[];
  total: number;
  pagina: number;
  tamanoPagina: number;
}

export interface ResumenPanel {
  totalAnimales: number;
  animalesEnTratamiento: number;
  alertasAbiertas: number;
  alertasGravesAbiertas: number;
  dietasEnPropuesta: number;
}

export interface Alerta {
  id: number;
  animalId: number;
  animalNombre: string;
  especieNombreComun: string;
  fecha: string;
  gravedad: string;
  tipo: string;
  mensaje: string;
  estado: string;
  origen: string;
  destacada: boolean;
}

export interface AnimalLista {
  id: number;
  nombre: string;
  especieId: number;
  especieNombreComun: string;
  sexo: string;
  fechaNacimiento: string;
  edadAnios: number;
  pesoKg: number;
  recinto: string;
  estado: string;
  fotoUrl: string | null;
  alertasAbiertas: number;
  tieneAlertaGrave: boolean;
}

export interface EspecieResumen {
  id: number;
  nombreComun: string;
  nombreCientifico: string;
  grupo: string;
  tipoDieta: string;
}

export interface Especie extends EspecieResumen {
  estadoConservacion: string;
  numeroAnimales: number;
  numeroDietas: number;
}

export interface ConsultaResumen {
  id: number;
  animalId: number;
  animalNombre: string;
  fecha: string;
  veterinario: string;
  motivo: string;
  diagnostico: string;
}

export interface Tratamiento {
  id: number;
  consultaId: number;
  animalId: number;
  animalNombre: string;
  medicamento: string;
  dosis: string;
  fechaInicio: string;
  fechaFin: string | null;
  notas: string | null;
  activo: boolean;
}

export interface ConsultaDetalle extends ConsultaResumen {
  pesoKg: number | null;
  temperatura: number | null;
  tratamientos: Tratamiento[];
}

export interface AnimalDetalle {
  id: number;
  nombre: string;
  especieId: number;
  especie: EspecieResumen;
  sexo: string;
  fechaNacimiento: string;
  edadAnios: number;
  pesoKg: number;
  recinto: string;
  estado: string;
  fotoUrl: string | null;
  alertasAbiertas: number;
  tieneAlertaGrave: boolean;
  tratamientosActivos: number;
  ultimaConsulta: ConsultaResumen | null;
  dietaValidadaId: number | null;
}

export interface HistorialAnimal {
  animal: {
    id: number;
    nombre: string;
    especieNombreComun: string;
    recinto: string;
  };
  consultas: ConsultaDetalle[];
  alertas: Alerta[];
  alimentacion: RegistroAlimentacion[];
}

export interface Alimento {
  id: number;
  nombre: string;
  categoria: string;
  kcalPorKg: number;
}

export interface DietaLinea {
  id: number;
  alimentoId: number;
  alimentoNombre: string;
  categoria: string;
  cantidadKgDia: number;
  frecuencia: string;
  kcalDia: number;
}

export interface DietaLista {
  id: number;
  especieId: number;
  especieNombreComun: string;
  nombre: string;
  estado: string;
  validadaPor: string | null;
  origen: string;
  numeroLineas: number;
  kcalTotalesDia: number;
  puedeAplicarse: boolean;
}

export interface Dieta extends DietaLista {
  lineas: DietaLinea[];
}

export interface RegistroAlimentacion {
  id: number;
  animalId: number;
  animalNombre: string;
  alimentoId: number;
  alimentoNombre: string;
  fecha: string;
  cantidadKg: number;
  consumido: number;
  kcalAportadas: number;
  coincideConDietaValidada: boolean;
}
