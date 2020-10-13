import { ReturnStatement } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RegistrationValidators } from './registration.validators';
@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  form: FormGroup;
  document: Document;

  constructor(private formBuilder: FormBuilder) { }

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
    //this.form.reset();
  }
}
