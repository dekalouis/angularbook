import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '../material.module';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, NavbarComponent, RouterModule, MaterialModule],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.css',
})
export class BookListComponent {
  books: any[] = [];
  errorMessage: string = '';
  constructor(private http: HttpClient) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.errorMessage = 'You are not logged in.';
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http
      .get<any[]>('http://localhost:5058/api/book', { headers })
      .subscribe({
        next: (res) => {
          this.books = res;
        },
        error: (err) => {
          this.errorMessage = 'Failed to load books. Please try again later.';
        },
      });
  }

  toggleRead(id: number) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http
      .patch(`http://localhost:5058/api/book/${id}/toggle`, {}, { headers })
      .subscribe({
        next: () => this.ngOnInit(), //reload the books
        error: (err) => {
          this.errorMessage =
            'Failed to update book status. Please try again later.';
        },
      });
  }

  deleteBook(id: number) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    this.http
      .delete(`http://localhost:5058/api/book/${id}`, { headers })
      .subscribe({
        next: () => this.ngOnInit(), //reload the books
        error: (err) => {
          this.errorMessage = 'Failed to delete book. Please try again later.';
        },
      });
  }

  updateBook(id: number) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    const newTitle = prompt('Enter new title:');
    const newAuthor = prompt('Enter new author:');
    if (!newTitle || !newAuthor) {
      this.errorMessage = 'Title and author cannot be empty.';
      return;
    }

    this.http
      .put(
        `http://localhost:5058/api/book/${id}`,
        {
          title: newTitle,
          author: newAuthor,
        },
        { headers }
      )
      .subscribe({
        next: () => this.ngOnInit(), //reload the books
        error: (err) => {
          this.errorMessage = 'Failed to update book. Please try again later.';
        },
      });
  }

  logout() {
    localStorage.removeItem('token');
    window.location.href = '/login'; // or use router.navigate
  }
}
