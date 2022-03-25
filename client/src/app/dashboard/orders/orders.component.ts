import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { IOrder } from 'src/app/shared/models/order';
import { IUser } from 'src/app/shared/models/user';
import { DashboardService } from '../dashboard.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  constructor(private dashboardService: DashboardService, private toastr: ToastrService, private accountService: AccountService) { }
orders: IOrder[]
statuses: string[]
element: HTMLElement;
currentUser$: Observable<IUser>;
breadcrumb$: Observable<any[]>;
  ngOnInit(): void {
    this.getOrders();
    this.getStatus();
    this.currentUser$=this.accountService.currentUser$;
  }
  getOrders() {
    this.dashboardService.getAllOrders().subscribe((orders : IOrder[]) => {
      this.orders = orders;
    }, error => {
      console.log(error);
    });
  }
  getStatus() {
    this.dashboardService.getAllStatus().subscribe((status : string[]) => {
      this.statuses = status;
      console.log(status);
    }, error => {
      console.log(error);
    });
  }

  changeOrderStatus(id:number){
  var order = this.orders.find((order: IOrder)=>{
      return order.id==id
    });
    
    var selected = (<HTMLInputElement>document.getElementById(order.id.toString())).value;
    order.status=selected
    this.dashboardService.changeOrderStatus(order)
    .subscribe( response=>{
    },error=>{
      this.toastr.success("Zmioniono status na "+order.status.toString())
    });
  }
  getValue(subtotal: number, shipping: number) {
    return (subtotal + shipping).toFixed(2);
  } 
  logout(){
    this.accountService.logout();
  }
}
