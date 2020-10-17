import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AddNewScientificPaperRequest } from '../model/addNewScientificPaperRequest';
@Component({
  selector: 'app-new-scpaper',
  templateUrl: './new-scpaper.component.html',
  styleUrls: ['./new-scpaper.component.css']
})
export class NewScpaperComponent implements OnInit {

  selectedFileName = "";
  fileUrl: string;
  fileToUpload: File;

  url = "https://localhost:44370/add";
  request: AddNewScientificPaperRequest;

  constructor(private httpClient: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit(value) {
    this.request = new AddNewScientificPaperRequest();
    this.request.magazineTitle = value['magazineTitle'];
    this.request.title = value['title'];
    this.request.authorFirstname = value['authorFirstname'];
    this.request.authorLastname = value['authorLastname'];
    this.request.keyword = value['keyword'];
    this.request.scientificField = value['scientificField'];

    let formData: FormData = new FormData();
    formData.append('file', this.fileToUpload);
    formData.append('json', JSON.stringify(this.request));

    this.httpClient.post(this.url, formData).subscribe(
      () => { },
      (error) => {
        alert(error.message);
      }
    );

    this.router.navigate(['/']);
  }

  handleFileInput(file: FileList) {
    this.fileToUpload = file.item(0);
    var reader = new FileReader();
    reader.onload = (event: any) => {
      this.fileUrl = event.target.result;
    };
    reader.readAsDataURL(this.fileToUpload);
    this.selectedFileName = this.fileToUpload.name;
  }
}
