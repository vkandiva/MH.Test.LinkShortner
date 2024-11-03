// src/app/app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { UrlListComponent } from './components/url-list/url-list.component';
import { UrlInputComponent } from './components/url-input/url-input.component';

@NgModule({
  declarations: [
    AppComponent,
    UrlListComponent,
    UrlInputComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
