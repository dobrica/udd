import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SearchScientificPaperRequest } from '../model/searchScientificPaperRequest';
import { SearchResult } from '../model/searchResult';

@Component({
  selector: 'app-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.css']
})
export class SearchFormComponent implements OnInit {

  mltEnabled = false;
  url = "https://localhost:44370/search";
  request: SearchScientificPaperRequest;
  searchResults: Array<SearchResult>;
  selectedFileName = "";
  fileUrl: string;
  fileToUpload: File;

  constructor(private httpClient: HttpClient, private router: Router) { }

  ngOnInit(): void { }

  onSubmit(value) {
    this.request = new SearchScientificPaperRequest();
    this.request.magazineTitle = value['magazineTitle'];
    this.request.title = value['title'];
    this.request.authorFirstname = value['authorFirstname'];
    this.request.authorLastname = value['authorLastname'];
    this.request.keyword = value['keyword'];
    this.request.scientificField = value['scientificField'];
    this.request.content = value['content'];
    if (value['operation']) {
      this.request.operation = 'AND';
    } else {
      this.request.operation = 'OR';
    }
    this.request.moreLikeThisEnabled = this.mltEnabled;
    this.request.moreLikeThisQuery = value['moreLikeThisQuery'];

    let formData: FormData = new FormData();
    formData.append('file', this.fileToUpload);
    formData.append('json', JSON.stringify(this.request));

    this.httpClient.post(this.url, this.request, { responseType: 'json' }).subscribe(
      (response: any) => {
        this.searchResults = new Array<SearchResult>();
        for (let entry of response) {
          let id = entry.Source.ScientificPaper.Id;
          let highlighter = '';
          highlighter += this.check(entry.Highlight['scientificPaper.magazineTitle']);
          highlighter += this.check(entry.Highlight['scientificPaper.title']);
          highlighter += this.check(entry.Highlight['scientificPaper.authors.firstname']);
          highlighter += this.check(entry.Highlight['scientificPaper.authors.lastname']);
          highlighter += this.check(entry.Highlight['scientificPaper.keywords.title']);
          highlighter += this.check(entry.Highlight['scientificPaper.scientificFields.title']);
          highlighter += this.check(entry.Highlight['attachment.content']);
          let title = entry.Source.ScientificPaper.Title;
          let searchResult = new SearchResult();
          searchResult.id = id;
          searchResult.highlight = highlighter;
          searchResult.title = title;
          searchResult.downloadLink = "https://localhost:44370/file/".concat(id);
          this.searchResults.push(searchResult);
        }
      },
      (error) => {
        alert(error.message);
      }
    );
  }

  stringToHTML(str) {
    var parser = new DOMParser();
    var doc = parser.parseFromString(str, 'text/html');
    return doc.body;
  };

  check(x): string {
    if (x == null) {
      return '';
    } else if (x == 'undefined') {
      return '';
    } else {
      return '... ' + x + ' ... <br>';
    }
  }

  mltUpdate() {
    this.mltEnabled = !this.mltEnabled;
  }
}


