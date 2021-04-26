import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { HttpErrorResponse } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../user/auth.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  pageTitle = 'Log In';

  maskUserName: boolean;
  userName: string = '';
  password: string = '';
  errorMessage: string = '';


  constructor(private authService: AuthService, private router: Router, private http: HttpClient) { }

  ngOnInit(): void {

  }

  cancel(): void {
    this.router.navigate(['dashboard']);
  }

  checkChanged(): void {
    this.maskUserName = !this.maskUserName;
  }

  login(loginForm: NgForm): void {
    if (loginForm && loginForm.valid) {
      this.authService.validateUser(this.userName, this.password).subscribe(result => {

        if (result.state == 0) //Success
        {
          this.authService.loginUser(this.userName, result.data);
          //localStorage.setItem("userName", JSON.stringify(this.userName));
          this.router.navigate(['/dashboard']);
        }
        this.errorMessage = '';
      });
    }
  }
}
