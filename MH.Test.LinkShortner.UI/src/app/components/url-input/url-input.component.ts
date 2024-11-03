// Code file for Url Input component
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'url-input',
  templateUrl: './url-input.component.html',
  styleUrls: ['../../app.component.css']
})
export class UrlInputComponent {
  originalUrl: string = '';
  @Output() urlSubmitted = new EventEmitter<string>();

  submitUrl(): void {
    if (this.originalUrl) {
      this.urlSubmitted.emit(this.originalUrl);
      this.originalUrl = ''; // Clear the input after submission
    }
  }
}
