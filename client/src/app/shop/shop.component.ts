import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';
import { ISystemType } from '../shared/models/systemType';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild("search", {static: false}) searchTerm: ElementRef;

  products: IProduct[];
  systems: ISystemType[];
  types: IType[];
  shopParams= new ShopParams();
  totalCount:number;
  sortOptions =[
    {name: 'Alfabetycznie', value: 'name'},
    {name: 'Cena: najtańsza => najdroższa', value: 'priceAsc'},
    {name: 'Cena: najdroższa => najtańsza', value: 'priceDesc'},
  ]

  constructor(private shopService: ShopService) { }

  ngOnInit() {
    this.getProducts();
    this.getSystems();
    this.getTypes();
  }
  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe(response =>{
      this.products = response.data;
      this.shopParams.pageNumber=response.pageIndex;
      this.shopParams.pageSize=response.pageSize;
      this.totalCount=response.count;
    }, error =>{
      console.log(error);
    });
  }
  getSystems(){
    this.shopService.getSystems().subscribe(response =>{
      this.systems = [{id:0,name:'All'}, ...response];
    }, error =>{
      console.log(error);
    });
  }
  getTypes(){
    this.shopService.getTypes().subscribe(response =>{
      this.types =  [{id:0,name:'All'}, ...response];
    }, error =>{
      console.log(error);
    });
  }
  onSizeSelected(systemId: number){
    this.shopParams.systemId=systemId;
    this.shopParams.pageNumber=1
    this.getProducts();
  }
  onTypeSelected(typeId: number){
    this.shopParams.typeId=typeId;
    this.shopParams.pageNumber=1
    this.getProducts();
  }
  onSortSelected(sort: string){
    this.shopParams.sort=sort;
    this.getProducts();
  }
  onPageChanged(event:any){
    if(this.shopParams.pageNumber !== event){
      this.shopParams.pageNumber=event;
      this.getProducts();
    }
  }
  onSearch(){
    this.shopParams.search= this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber=1
    this.getProducts();
  }
  onReset(){
    this.searchTerm.nativeElement.value="";
    this.shopParams= new ShopParams();
    this.getProducts();
  }
}
