import { ReturnStatement } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RegistrationValidators } from './registration.validators';
import { RegistrationService } from '../../services/registration/registration.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  form: FormGroup;
  document: Document;

  constructor(private formBuilder: FormBuilder, private registrationService: RegistrationService) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      email: new FormControl('', [
        Validators.email,
        Validators.required
      ], [
        RegistrationValidators.uniqEmail
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8)
      ]),
      confirmPassword: new FormControl('', [
        Validators.required,
      ]),
      username: new FormControl('', [
        Validators.required
      ], [
        RegistrationValidators.uniqUsername
      ]),
      firstname: new FormControl('', [
        Validators.required
      ]),
      lastname: new FormControl('', [
        Validators.required
      ])
    }, {
      validators: RegistrationValidators.mustMatch('password', 'confirmPassword')
    });
  }

  submit(): void {
    console.log('Form created: ', this.form);
    console.log(this.form.value);

    const formData = new FormData();
    formData.append('email', this.form.get('email').value);
    formData.append('username', this.form.get('username').value);
    formData.append('firstname', this.form.get('firstname').value);
    formData.append('lastname', this.form.get('lastname').value);
    formData.append('password', this.form.get('password').value);

    this.registrationService.uploadForm(formData).subscribe(result => {
      console.log('POST: ', result);
    });

    this.form.reset();
  }
}
