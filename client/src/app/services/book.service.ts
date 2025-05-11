import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap, switchMap, shareReplay } from 'rxjs/operators';
import { SnackbarService } from './snackbar.service';
import { environment } from '../../environments/environment';

export interface Book {
  id: number;
  title: string;
  author: string;
  isRead: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class BookService {
  private readonly API_URL = `${environment.apiBaseUrl}/api/book`;
  private refreshTrigger$ = new BehaviorSubject<void>(undefined);

  private isLoading$ = new BehaviorSubject<boolean>(false);
  // Create an observable for the loading state
  loading$ = this.isLoading$.asObservable();

  books$ = this.refreshTrigger$.pipe(
    switchMap(() => {
      const token = localStorage.getItem('token');
      if (!token) {
        return throwError(() => new Error('You are not logged in.'));
      }
      /*
      actually better if we do this to not break the stream
      import { EMPTY } from 'rxjs';

      if (!token) return EMPTY;
      */

      const headers = new HttpHeaders({
        Authorization: `Bearer ${token}`,
      });

      return this.http.get<Book[]>(this.API_URL, { headers }).pipe(
        tap(() => console.log('[BookService] Fetched books')),
        catchError((error) => {
          this.snackbar.show(error.message || 'Failed to fetch books');
          return throwError(() => error);
        })
      );
    }),
    shareReplay(1)
  );

  constructor(private http: HttpClient, private snackbar: SnackbarService) {}

  getBooks(): Observable<Book[]> {
    return this.books$;
  }

  refreshBooks(): void {
    this.refreshTrigger$.next();
  }
  addBook(title: string, author: string): Observable<any> {
    const token = localStorage.getItem('token');
    if (!token) {
      return throwError(() => new Error('You are not logged in.'));
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.post(this.API_URL, { title, author }, { headers }).pipe(
      tap(() => {
        this.snackbar.show('Book added successfully');
        // Explicitly trigger refresh before returning
        this.refreshTrigger$.next();
      }),
      catchError((error) => {
        this.snackbar.show('Failed to add book');
        return throwError(() => error);
      })
    );
  }

  toggleRead(id: number): Observable<any> {
    const token = localStorage.getItem('token');
    if (!token) {
      return throwError(() => new Error('You are not logged in.'));
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http
      .patch(`${this.API_URL}/${id}/toggle`, {}, { headers })
      .pipe(
        tap(() => {
          this.snackbar.show('Book updated');
          this.refreshBooks();
        }),
        catchError((error) => {
          this.snackbar.show('Failed to toggle book');
          return throwError(() => error);
        })
      );
  }

  deleteBook(id: number): Observable<any> {
    const token = localStorage.getItem('token');
    if (!token) {
      return throwError(() => new Error('You are not logged in.'));
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.delete(`${this.API_URL}/${id}`, { headers }).pipe(
      tap(() => {
        this.snackbar.show('Book deleted');
        this.refreshBooks();
      }),
      catchError((error) => {
        this.snackbar.show('Failed to delete book');
        return throwError(() => error);
      })
    );
  }

  updateBook(id: number, title: string, author: string): Observable<any> {
    const token = localStorage.getItem('token');
    if (!token) {
      return throwError(() => new Error('You are not logged in.'));
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http
      .put(
        `${this.API_URL}/${id}`,
        {
          title,
          author,
        },
        { headers }
      )
      .pipe(
        tap(() => {
          this.snackbar.show('Book updated');
          this.refreshTrigger$.next();
        }),
        catchError((error) => {
          this.snackbar.show('Failed to update book');
          return throwError(() => error);
        })
      );
  }
}
