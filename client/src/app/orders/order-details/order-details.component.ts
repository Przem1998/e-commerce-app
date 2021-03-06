import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  order: IOrder;
  constructor(private route: ActivatedRoute, private breadcrumbService: BreadcrumbService, private ordersService: OrdersService) { 
    this.breadcrumbService.set('@OrderDetailed','');
  }

  ngOnInit() {
    this.ordersService.getOrderDetailed(+this.route.snapshot.paramMap.get('id'))
    .subscribe((order: IOrder) =>{
      this.order= order;
      console.log(this.order.orderItems);
      this.breadcrumbService.set('@OrderDetailed', `Zamówienie nr ${order.id} - ${order.status}`);
    }, error => {
      console.log(error);
    })
  }
  NumerToFixTwo(price:number):String{
    return price.toFixed(2);
  }
}
