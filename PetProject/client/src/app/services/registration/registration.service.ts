import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReactiveFormsModule  } from '@angular/forms';
import { UserManager, UserManagerSettings } from 'oidc-client';
import { User } from '../../models/user';
import { AbstractControl, AsyncValidatorFn, ValidationErrors} from '@angular/forms';
import { map } from 'rxjs/operators';


const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  uploadForm(userdata: FormData): Observable<any> {
    return this.http.post<any>(this.baseUrl + 'account/register', userdata);
  }

  checkEmailAvailability(email: string): Observable<boolean> {
    const body = JSON.stringify(email);
    console.log(body);
    return this.http.post<boolean>(this.baseUrl + 'Account/CheckEmailIsAvailability', body, httpOptions);
  }

  checkUsernameAvailability(userneme: string): Observable<boolean> {
    const body = JSON.stringify(userneme);
    return this.http.post<boolean>(this.baseUrl + 'Account/CheckUsernameIsAvailability', body, httpOptions);
  }
}


export function getClientSetting(): UserManagerSettings {
  return {
    authority: 'http://localhost:5000',
    client_id: 'angular_spa',
    redirect_uri: 'http://localhost:4200/auth-callback',
    response_type: 'id_token token',
    scope: 'openid profile email api.read'
  }
}
