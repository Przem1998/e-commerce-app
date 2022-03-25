import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { AccountService } from 'src/app/account/account.service';
import { IUser } from 'src/app/shared/models/user';

@Component({
  selector: 'app-admin-nav-bar',
  templateUrl: './admin-nav-bar.component.html',
  styleUrls: ['./admin-nav-bar.component.scss']
})
export class AdminNavBarComponent implements OnInit {

  currentUser$: Observable<IUser>;
  breadcrumb$: Observable<any[]>;
  constructor(private accountService:AccountService) { }

  ngOnInit(): void {
    this.currentUser$=this.accountService.currentUser$;
  }
  logout(){
    this.accountService.logout();
  }

}
