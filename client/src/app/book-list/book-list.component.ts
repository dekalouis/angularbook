import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '../material.module';
import { SnackbarService } from '../services/snackbar.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../components/confirm-dialog/confirm-dialog.component';

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
  constructor(
    private http: HttpClient,
    private snackbar: SnackbarService,
    private dialog: MatDialog
  ) {}

  sortBooks(books: any[]): any[] {
    //Sort consistently by ID only
    return books.sort((a, b) => a.id - b.id);
  }

  // //add sorting by id
  // sortBooks(books: any[]): any[] {
  //   // Sort by isRead (false first), then by id
  //   return books.sort((a, b) => {
  //     // First compare by isRead status
  //     if (a.isRead !== b.isRead) {
  //       return a.isRead ? 1 : -1; // false comes before true
  //     }

  //     // If isRead status is the same, sort by ID
  //     return a.id - b.id;
  //   });
  // }

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
          // this.books = res;
          this.books = this.sortBooks(res);
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
        next: () => {
          this.snackbar.show(
            'Book status updated successfully',
            'Dismiss',
            2000
          );

          // this.ngOnInit(); //reload the books

          // Update just the toggled book in place - no need to resort since we're only sorting by ID
          const book = this.books.find((b) => b.id === id);
          if (book) {
            book.isRead = !book.isRead;
          }
          //!SORT BY THE FRONTEND with isread
          // const book = this.books.find((b) => b.id === id);
          // if (book) {
          //   book.isRead = !book.isRead;
          //   // Resort the books after toggling
          //   this.books = this.sortBooks(this.books);
          // }
        },
        error: (err) => {
          this.snackbar.show('Failed to toggle read status.', 'Dismiss');
          this.errorMessage =
            'Failed to toggle read status. Please try again later.';
        },
      });
  }

  deleteBook(id: number) {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to delete this book?' },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.http
          .delete(`http://localhost:5058/api/book/${id}`, { headers })
          .subscribe({
            next: () => {
              this.snackbar.show('Book deleted successfully');
              this.books = this.books.filter((book) => book.id !== id);
            },
            error: (err) => {
              this.snackbar.show('Failed to delete the book.', 'Dismiss', 4000);
              this.errorMessage =
                'Failed to delete book. Please try again later.';
            },
          });
      }
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
