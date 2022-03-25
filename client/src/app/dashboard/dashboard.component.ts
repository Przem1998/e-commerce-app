import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account/account.service';
import { IOrderRaport } from '../shared/models/orderRaport';
import { SharedModule } from '../shared/shared.module';
import { DashboardService } from './dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
 selectedOptionStatus:string
  constructor(private accountService: AccountService, private dashboardService: DashboardService) { }

  ordersRaport:IOrderRaport
  ngOnInit(): void {
  this.getOrdersRaport();
  }
  getOrdersRaport(){
    this.dashboardService.getOrdersRaport().subscribe((res:IOrderRaport)=>{
      this.ordersRaport=res;
    })
  }
}
