import { AbstractControl, FormGroup, AsyncValidatorFn, ValidationErrors} from '@angular/forms';
import { RegistrationService } from '../../services/registration/registration.service';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

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

export function UniqEmail(registrationService: RegistrationService): AsyncValidatorFn  {
    return (control: AbstractControl): Observable<ValidationErrors> | null => {
        const email = control.value;

        if (!email.includes('@')) {
             return null;
        }
        console.log('work');
        return registrationService.checkEmailAvailability(email).pipe(
            tap((result: boolean) => {
                console.log(result ? {uniqemail: {value : control.value}} : null);
            }),
            map((result: boolean) => {
                console.log(result ? {uniqemail: {value : control.value}} : null);
                return result ? {uniqemail: {value : control.value}} : null;
            }));
    };
}

export function UniqUsername(registrationService: RegistrationService): AsyncValidatorFn {
    return (control: AbstractControl): any | null => {
        const username = control.value;

        if (username === ''){
            return null;
        }

        registrationService.checkUsernameAvailability(username).subscribe(result => {
            return !result ? {uniqusername: {value : control.value}} : null;
        });
    };
}
