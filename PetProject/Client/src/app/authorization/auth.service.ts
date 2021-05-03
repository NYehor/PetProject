import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { User } from 'src/app/models/user';


const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {

  private baseUrl: string;
  private loggedUser: User;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.baseUrl = baseUrl;
  }

  isAuthenticated(): Observable<boolean> {
    return this.getCurrentUser().pipe(
      map(user => !!user),
      catchError(() => of(false))
    );
  }

  login(email: string, password: string): Observable<boolean> {
    const body = { Email: email, Password: password};
    return this.http.post<boolean>(this.baseUrl + 'Account/Login', body, httpOptions);
  }

  logout(): Observable<boolean> {
    this.loggedUser = undefined;
    return this.http.post<any>(this.baseUrl + 'Account/Logout', httpOptions);
  }

  confirmEmail(email: string, code: string): Observable<void> {
    const body = { Email: email, Code: code};
    return this.http.post<any>(this.baseUrl + '', body, httpOptions);
  }

  getCurrentUser(): Observable<User> {
    if (this.loggedUser) {
      return of(this.loggedUser);
    }
    else {
      return this.http.get<User>(this.baseUrl + 'Account/Login')
        .pipe(tap(user => this.loggedUser = user));
    }

    return this.http.get<User>(this.baseUrl + '').pipe(tap(user => this.loggedUser = user));
  }

  uploadForm(userRegistration: FormData): Observable<any> {
    return this.http.post<any>(this.baseUrl + 'account/Registration', userRegistration);
  }

  checkEmailAvailability(email: string): Observable<boolean> {
    const body = JSON.stringify(email);
    return this.http.post<boolean>(this.baseUrl + 'Account/CheckEmailIsAvailability', body, httpOptions);
  }

  checkUsernameAvailability(userneme: string): Observable<boolean> {
    const body = JSON.stringify(userneme);
    return this.http.post<boolean>(this.baseUrl + 'Account/CheckUsernameIsAvailability', body, httpOptions);
  }
}
