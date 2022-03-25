import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { forkJoin, Observable, of } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';
import { BusyService } from './core/services/busy.service';
import { IBasket } from './shared/models/basket';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  routes$: Observable<any[]>;
  title = 'Sklep hydrauliczny - beta';
  loading = true;

  constructor(
      private basketService: BasketService,
      private accountService: AccountService,
      private busyService: BusyService,
      private breadcrumbService: BreadcrumbService) {
      this.busyService.busy();
  }

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const basketId = localStorage.getItem('basket_id');
    const sources = [
      token ? this.accountService.loadCurrentUser(token) : of(null),
      basketId ? this.basketService.getBasket(basketId) : of(null)
    ];
    forkJoin([
      ...sources
    ]).subscribe(() => { }, error => console.log(error), () => {
      console.log('app loaded');
      this.loading = false;
      this.busyService.idle();
    });
  }

  loadBasket(){
    const basketId = localStorage.getItem('basket_id');
    if (basketId){
      this.basketService.getBasket(basketId).subscribe(() =>{
      
        console.log('initialised basket');
      }, error => {
        console.log(error);
      });
    }
  }
  loadCurrentUser(){
    const token = localStorage.getItem('token');

      this.accountService.loadCurrentUser(token).subscribe(()=>{
        console.log('loaded user')

      }, error => {
        console.log(error);
      })
    
    
  }
}
