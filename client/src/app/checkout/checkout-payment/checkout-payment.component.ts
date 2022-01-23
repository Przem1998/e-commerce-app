import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrder } from 'src/app/shared/models/order';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, private toastr: ToastrService, private router: Router, private account: AccountService) { }

  ngOnInit(): void {
  }
  submitOrder(){
    const basket = this.basketService.getCurrentBasketValue();
    const orderToCreate = this.getOrderToCreate(basket);
    console.log(orderToCreate);



    this.checkoutService.createOrder(orderToCreate).subscribe((order: IOrder) => {
      this.toastr.success('Order created successfully');
      this.basketService.deleteLocalBasket(basket.id);
      console.log(order);
      const navigationExtras: NavigationExtras = {state: order};
      this.router.navigate(['checkout/success'], navigationExtras);
    }, error => {
      this.toastr.error(error.message);
      console.log(error);
    });


  }
    // createPayment(){
  //   return this.http.post(this.baseUrl+'payment/'+this.getCurrentBasketValue().id,{})
  //   .pipe(
  //     map((basket:IBasket)=>{
  //       console.log(this.getCurrentBasketValue())
  //       this.basketSource.next(basket);
  //       console.log(this.getCurrentBasketValue());
  //     })
  //   );
  // }
  goToPayu(){
    this.checkoutService.createPayment(this.getOrderToCreate(this.basketService.getCurrentBasketValue())).subscribe((response: any) =>{

      window.open(response,"_blank");

    }, error =>{
      console.log(error);
    });
  }
  private getOrderToCreate(basket: IBasket) {
    return{
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    };
  }

  referenceToPayu(){
    
  }
}
