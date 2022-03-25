import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { IProduct } from 'src/app/shared/models/product';
import { IType } from 'src/app/shared/models/productType';
import { ShopParams } from 'src/app/shared/models/shopParams';
import { ISystemType } from 'src/app/shared/models/systemType';
import { IUser } from 'src/app/shared/models/user';
import { ShopService } from 'src/app/shop/shop.service';
import { DashboardService } from '../dashboard.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  currentUser$: Observable<IUser>;
  breadcrumb$: Observable<any[]>;
  constructor(private shopService: ShopService, private dashboardService:DashboardService, private toastr:ToastrService, private router:Router, private route:ActivatedRoute, private accountService:AccountService) { }
  products: IProduct[]; 
  system: ISystemType[];
  type: IType[];
  shopParams= new ShopParams();
  totalCount:number;
  ngOnInit(): void {
    this.currentUser$=this.accountService.currentUser$;
    this.getProducts();
  }
  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe(response=> {
      this.products=response.data;
      this.shopParams.pageNumber=response.pageIndex;
      this.shopParams.pageSize=response.pageSize;
      this.totalCount=response.count;
    }, error => {
      console.log(error);
    });
  }
  NumerToFixTwo(price:number):String{
    return price.toFixed(2);
  }
  removeProduct(productItem: IProduct){
    this.dashboardService.removeProduct(productItem).subscribe(res=>{
      },error =>{
        this.toastr.success("Product '"+productItem.name+"' usunięto");
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
        this.router.onSameUrlNavigation = 'reload';
        this.router.navigate(['./'], {
          relativeTo:this.route
        })
      })

  }
  
  onPageChanged(event:any){
    if(this.shopParams.pageNumber !== event){
      this.shopParams.pageNumber=event;
      this.getProducts();
    }
  }
  logout(){
    this.accountService.logout();
  }
}


