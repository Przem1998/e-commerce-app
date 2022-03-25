import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { OrdersService } from 'src/app/orders/orders.service';
import { IOrder } from 'src/app/shared/models/order';
import { IUser } from 'src/app/shared/models/user';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  
  order: IOrder;
  currentUser$: Observable<IUser>;
breadcrumb$: Observable<any[]>;
  constructor(private route: ActivatedRoute, private breadcrumbService: BreadcrumbService, private ordersService: OrdersService, private accountService:AccountService) { 
    this.breadcrumbService.set('@OrderDetailed','');
  }

  ngOnInit() {
    this.currentUser$=this.accountService.currentUser$;
    this.ordersService.getOrderDetailed(+this.route.snapshot.paramMap.get('id'))
    .subscribe((order: IOrder) =>{
      this.order= order;
      console.log(this.order.orderItems);
      this.breadcrumbService.set('@OrderDetailed', `Zamówienie  nr ${order.id} - ${order.status}`);
    }, error => {
      console.log(error);
    })
  }
  NumerToFixTwo(price:number):String{
    return price.toFixed(2);
  }
  logout(){
    this.accountService.logout();
  }
}
