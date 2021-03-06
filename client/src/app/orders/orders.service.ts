import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseurl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getOrderForUser(){
    return this.http.get(this.baseurl+'order');
  }
  getOrderDetailed(id:number){
    return this.http.get(this.baseurl+'order/'+id);
  }
  getValueMultiplyPriceByQuantity(price: number, quantity: number) {
    return (price * quantity).toFixed(2);
  } 
}
