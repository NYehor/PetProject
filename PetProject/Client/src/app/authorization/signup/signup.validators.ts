import { AbstractControl, FormGroup, AsyncValidatorFn, ValidationErrors} from '@angular/forms';
import { Observable, throwError } from 'rxjs';
import { AuthorizeService } from '../auth.service';
import { map, catchError } from 'rxjs/operators';

export class PasswordValidators {
    static mustMatch(password: string, passwordConfirm: string): any {
        return (formGroup: FormGroup) => {
            const control = formGroup.controls[password];
            const matchingControl = formGroup.controls[passwordConfirm];

            if (matchingControl.errors && !matchingControl.errors.mustMatch) {
                return;
            }

            if (control.value !== matchingControl.value) {
                matchingControl.setErrors({ mustMatch: true});
            }
            else {
                matchingControl.setErrors(null);
            }
          };
    }
}

export function UniqEmail(authService: AuthorizeService): AsyncValidatorFn  {
    return (control: AbstractControl): Observable<ValidationErrors> | null => {
        const email = control.value;

        if (!email.includes('@')) {
             return null;
        }

        return authService.checkEmailAvailability(email).pipe(
            map((result: boolean) => {
                return !result ? {uniqemail: {value : control.value}} : null;
            }),
            catchError( error => {
                console.log(error);
                return throwError( 'Something went wrong!' );
              }));
    };
}

export function UniqUsername(authService: AuthorizeService): AsyncValidatorFn {
    return (control: AbstractControl): any | null => {
        const username = control.value;

        if (username === ''){
            return null;
        }

        return authService.checkUsernameAvailability(username).pipe(map((result: boolean) => {
            return !result ? {uniqusername: {value : control.value}} : null;
        }),
        catchError( error => {
            console.log(error);
            return throwError( 'Something went wrong!' );
          }));
    };
}
