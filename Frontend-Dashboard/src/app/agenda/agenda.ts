import { Component, EventEmitter, Output, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Audiencia {
  id: number;
  titulo: string;
  fecha: string;
  hora: string;
  juzgado: string;
  expediente: string;
  tipo: string;
}

@Component({
  selector: 'app-agenda',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './agenda.html',
  styleUrl: './agenda.css',
  encapsulation: ViewEncapsulation.None
})
export class AgendaComponent {
  audiencias: Audiencia[] = [
    { id: 1, titulo: 'Audiencia de Conciliación', fecha: '2026-04-24', hora: '09:00 AM', juzgado: 'Juzgado Civil 4', expediente: '2024-0042', tipo: 'Civil' },
    { id: 2, titulo: 'Declaración Testimonial', fecha: '2026-04-25', hora: '11:30 AM', juzgado: 'Juzgado Laboral 2', expediente: '2024-0115', tipo: 'Laboral' }
  ];

  nuevaAudiencia = {
    titulo: '',
    fecha: '',
    hora: '',
    juzgado: '',
    expediente: '',
    tipo: 'Civil'
  };

  showForm = false;
  showTypeDropdown = false;
  currentPage = 1;
  pageSize = 5;

  tiposAudiencia = ['Civil', 'Penal', 'Laboral', 'Comercial'];

  selectTipo(tipo: string) {
    this.nuevaAudiencia.tipo = tipo;
    this.showTypeDropdown = false;
  }

  get totalPages(): number {
    return Math.ceil(this.audiencias.length / this.pageSize);
  }

  get paginatedAudiencias(): Audiencia[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.audiencias.slice(start, start + this.pageSize);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) this.currentPage++;
  }

  prevPage() {
    if (this.currentPage > 1) this.currentPage--;
  }

  agregarAudiencia() {
    if (this.nuevaAudiencia.titulo && this.nuevaAudiencia.fecha) {
      this.audiencias.unshift({
        id: Date.now(),
        ...this.nuevaAudiencia
      });
      this.resetForm();
      this.currentPage = 1; // Volver a la primera página para ver el nuevo registro
    }
  }

  resetForm() {
    this.nuevaAudiencia = { titulo: '', fecha: '', hora: '', juzgado: '', expediente: '', tipo: 'Civil' };
    this.showForm = false;
  }

  eliminarAudiencia(id: number) {
    this.audiencias = this.audiencias.filter(a => a.id !== id);
    if (this.currentPage > this.totalPages && this.currentPage > 1) {
      this.currentPage--;
    }
  }
}
