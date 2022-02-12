import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IProduct } from 'src/app/shared/models/product';
import { IType } from 'src/app/shared/models/productType';
import { ShopParams } from 'src/app/shared/models/shopParams';
import { ISystemType } from 'src/app/shared/models/systemType';
import { ShopService } from 'src/app/shop/shop.service';
import { DashboardService } from '../dashboard.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {

  constructor(private shopService: ShopService, private dashboardService:DashboardService, private toastr:ToastrService, private router:Router, private route:ActivatedRoute) { }
  products: IProduct[]; 
  system: ISystemType[];
  type: IType[];
  shopParams= new ShopParams();
  totalCount:number;
  ngOnInit(): void {
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
  removeProduct(productItem: IProduct){
    this.dashboardService.removeProduct(productItem).subscribe(res=>{
      
    },error =>{
      console.log(error);
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
}
