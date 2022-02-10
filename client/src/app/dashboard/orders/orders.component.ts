import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { IOrder } from 'src/app/shared/models/order';
import { DashboardService } from '../dashboard.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  constructor(private dashboardService: DashboardService, private toastr: ToastrService) { }
orders: IOrder[]
statuses: string[]
element: HTMLElement;
  ngOnInit(): void {
    this.getOrders();
    this.getStatus();
  }
  getOrders() {
    this.dashboardService.getAllOrders().subscribe((orders : IOrder[]) => {
      
      this.orders = orders;
      console.log(orders);
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
    this.dashboardService.changeOrderStatus(order).subscribe( response=>{
    },error=>{
      this.toastr.success("Zmioniono status na "+order.status.toString())
    });
  }
  getValue(subtotal: number, shipping: number) {
    return (subtotal + shipping).toFixed(2);
  } 

}
