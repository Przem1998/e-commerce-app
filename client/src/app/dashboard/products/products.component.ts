import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/models/product';
import { IType } from 'src/app/shared/models/productType';
import { ShopParams } from 'src/app/shared/models/shopParams';
import { ISize } from 'src/app/shared/models/size';
import { ShopService } from 'src/app/shop/shop.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {

  constructor(private shopService: ShopService) { }
  products: IProduct[]; 
  size: ISize[];
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
  
  onPageChanged(event:any){
    if(this.shopParams.pageNumber !== event){
      this.shopParams.pageNumber=event;
      this.getProducts();
    }
  }
}
