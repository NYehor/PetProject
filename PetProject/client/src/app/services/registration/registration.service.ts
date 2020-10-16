import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReactiveFormsModule  } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  uploadForm(formData: FormData): Observable<any> {
    return this.http.post<any>(this.baseUrl + 'registration', formData);
  }
}
