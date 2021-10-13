import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null); // A wariant of Subject that requires an initial value and emits its currnt value whenever it is subcribed to
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();
  shipping =0;

  constructor(private http: HttpClient) { }

  setShippingPrice(deliveryMethod : IDeliveryMethod){
    this.shipping = deliveryMethod.price;
    this.calculateTotals()
  }

  getBasket(id: string){
    return this.http.get(this.baseUrl + 'basket?id=' + id ).pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
        this.calculateTotals();
        console.log(this.getCurrentBasketValue());
      })
    )
  }
  setBasket(basket: IBasket){
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: IBasket) =>{
      this.basketSource.next(response);
      console.log(response);
      this.calculateTotals();
    }, error => {
      console.log(error);
    });
  }
  getCurrentBasketValue(){
    return this.basketSource.value;
  }
  addItemToBasket(item: IProduct, quantity = 1){
    const itemToAdd : IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }
  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }
  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number):IBasketItem[] {

    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1){
      itemToAdd.quantity = quantity
      items.push(itemToAdd);
    }
    else{
      items[index].quantity += quantity;
    }
    return items;
  }
  private mapProductItemToBasketItem(item: IProduct, quantity: number) : IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      productType: item.productType,
      productSize: item.productSize
    }
  }
  private calculateTotals(){
    const basket = this.getCurrentBasketValue();
    const shipping = this.shipping;
    let middleCalc = basket.items.reduce((a,b) =>  b.price * b.quantity + a, 0).toFixed(2);
    const subtotal  = Number(middleCalc);
    middleCalc = (subtotal + shipping).toFixed(2);
    const total= Number(middleCalc);
    this.basketTotalSource.next({shipping, total, subtotal});
  }
  incrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }
  decrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if(basket.items[foundItemIndex].quantity >1){
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }else{
      this.removeItemFromBasket(item);
    }
   
  }
  getValueMultiplyPriceByQuantity(price: number, quantity: number) {
    return (price * quantity).toFixed(2);
  } 
  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket.items.some(x => x.id === item.id)){
      basket.items = basket.items.filter(i => i.id !== item.id);
      if(basket.items.length > 0){
        this.setBasket(basket);
      }else{
        this.deleteBasket(basket);
      }
    }
  }

  deleteLocalBasket(id: string){
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem(id);
  }
  
  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl+'basket?id=' +basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
    }, error => {
      console.log(error);
    });
  }
}
