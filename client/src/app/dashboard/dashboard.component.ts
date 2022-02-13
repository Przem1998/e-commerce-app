import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account/account.service';
import { SharedModule } from '../shared/shared.module';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
 selectedOptionStatus:string
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  
  }

}
