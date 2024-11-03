// src/app/services/url-shortener.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UrlRequest } from '../models/url-request.model';
import { environment } from '../environments/environment'; // Import environment

@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Method to create a shortened URL with error handling
  createShortenedUrl(originalUrl: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<UrlRequest>(`${this.apiUrl}/ShortenUrl`, { originalUrl: originalUrl }, { headers })
      .pipe(catchError(this.handleError));
  }

  // Method to fetch all shortened URLs with error handling
  getAllShortenedUrls(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/ShortenUrl`)
      .pipe(catchError(this.handleError));
  }

  // Generic error handling function
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Server returned code: ${error.status}, error message is: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }
}
