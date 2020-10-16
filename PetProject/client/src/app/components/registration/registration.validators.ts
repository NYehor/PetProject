import { FormControl, FormGroup} from '@angular/forms';
import { Observable } from 'rxjs';

export class RegistrationValidators {

    static uniqEmail(control: FormControl): Promise<any> | Observable<any> {
        return new Promise(resolve => {
            setTimeout(() =>
            {
                resolve(null);
                //resolve({uniqEmail: true});
            }, 1000);
        });
    }

    static uniqUsername(control: FormControl): Promise<any> | Observable<any> {
        return new Promise(resolve => {
            setTimeout(() =>
            {
                resolve(null);
                //resolve({uniqUsername: true});
            }, 1000);
        });
    }

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
