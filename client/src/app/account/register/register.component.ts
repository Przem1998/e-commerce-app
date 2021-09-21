import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[];

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }
  createRegisterForm(){
    this.registerForm = this.fb.group({
      displayName: [null, [Validators.required]],
      email: [null, 
             [Validators.required, Validators.pattern("^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$")],
             [this.validateEmailNotTaken()]
            ],
      password: [null, [Validators.required, Validators.pattern("(?=^.{8,16}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$")]]
    })
  }
  onSubmit(){
    this.accountService.register(this.registerForm.value).subscribe( response =>{
      this.router.navigateByUrl('/shop');
    }, error => {
      console.log(error);
      this.errors= error.errors;
      
    });
  }
  validateEmailNotTaken() : AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if(!control.value){
            return of(null);
          }
          return this.accountService.checkIfEmailExists(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null;
            })
          );
        })
      );
    }
  };
}
