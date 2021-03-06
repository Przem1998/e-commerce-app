import { Component, OnInit } from '@angular/core';
import { IOrder } from '../shared/models/order';
import { OrdersService } from './orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orders: IOrder[];
  constructor(private orderService: OrdersService) { }

  ngOnInit(): void {
    this.getOrders();
  }
  getOrders() {
    this.orderService.getOrderForUser().subscribe((orders : IOrder[]) => {
      
      this.orders = orders;
    }, error => {
      console.log(error);
    });
  }
  getValue(subtotal: number, shipping: number) {
    return (subtotal + shipping).toFixed(2);
  } 
  NumerToFixTwo(price:number):String{
    return price.toFixed(2);
  }
}
