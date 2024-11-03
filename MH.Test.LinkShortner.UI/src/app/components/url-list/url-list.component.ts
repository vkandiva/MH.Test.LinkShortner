// Code file for Url List component
import { Component, OnInit } from '@angular/core';
import { UrlShortenerService } from '../../services/url-shortener.service';
import { UrlMapping } from '../../models/url-mapping.model';

@Component({
  selector: 'url-list',
  templateUrl: './url-list.component.html',
  styleUrls: ['../../app.component.css']
})
export class UrlListComponent implements OnInit {
  urlMapping: UrlMapping[] = [];
  errorMessage: string = '';

  constructor(private urlShortenerService: UrlShortenerService) { }

  ngOnInit(): void {
    this.fetchShortenedUrls();
  }

  fetchShortenedUrls(): void {
    this.urlShortenerService.getAllShortenedUrls().subscribe({
      next: (urlMaps: UrlMapping[]) => {
        this.urlMapping = urlMaps;
        this.errorMessage = ''; // Clear any previous error messages
      },
      error: (error) => {
        this.errorMessage = error.message; // Display the error message
      }
    });
  }

  handleUrlSubmission(originalUrl: string): void {
    this.urlShortenerService.createShortenedUrl(originalUrl).subscribe({
      next: (result: UrlMapping) => {
        this.urlMapping.push(result);
        this.errorMessage = ''; // Clear any previous error messages
      },
      error: (error) => {
        this.errorMessage = error.message; // Display the error message
      }
    });
  }
}
