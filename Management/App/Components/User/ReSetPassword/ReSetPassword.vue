
  <template src="./ReSetPassword.html"></template>
  <script src="./ReSetPassword.js"></script>
  <style scoped="">
      /*Title*/
      .title {
          font-weight: 700;
          margin-right: 10px;
      }

      .h5, h5 {
          font-size: 1.57em;
          line-height: 1.4em;
          margin-bottom: 15px;
      }
      /*End Title*/
      .card {
          border: 0;
          border-radius: .1875rem;
          display: inline-block;
          position: relative;
          margin-bottom: 20px;
          -webkit-box-shadow: 0 1px 15px 1px rgba(39,39,39,.1);
          box-shadow: 0 1px 15px 1px rgba(39,39,39,.1);
      }
          /*profile*/
          .card .card-body {
              padding: 15px 15px 10px 15px;
          }

              .card .card-body.table-full-width {
                  padding-left: 0;
                  padding-right: 0;
              }

          .card .card-header {
              padding: 15px 15px 0;
              border: 0;
          }

              .card .card-header:not([data-background-color]) {
                  background-color: transparent;
              }

              .card .card-header .card-title {
                  margin-top: 10px;
              }

          .card .map {
              border-radius: 0.1875rem;
          }

              .card .map.map-big {
                  height: 400px;
              }

          .card[data-background-color="orange"] {
              background-color: #f96332;
          }

              .card[data-background-color="orange"] .card-header {
                  background-color: #f96332;
              }

              .card[data-background-color="orange"] .card-footer .stats {
                  color: #FFFFFF;
              }

          .card[data-background-color="red"] {
              background-color: #FF3636;
          }

          .card[data-background-color="yellow"] {
              background-color: #FFB236;
          }

          .card[data-background-color="blue"] {
              background-color: #2CA8FF;
          }

          .card[data-background-color="green"] {
              background-color: #18ce0f;
          }

          .card .image {
              overflow: hidden;
              height: 200px;
              position: relative;
          }

          .card .avatar {
              width: 30px;
              height: 30px;
              overflow: hidden;
              border-radius: 50%;
              margin-bottom: 15px;
          }

          .card label {
              font-size: 0.8571em;
              margin-bottom: 5px;
              color: #9A9A9A;
          }

          .card .card-footer {
              background-color: transparent;
              border: 0;
          }

              .card .card-footer .stats i {
                  margin-right: 5px;
                  position: relative;
                  top: 2px;
              }

              .card .card-footer .btn {
                  margin: 0;
              }

      .card-chart .card-header .card-title {
          margin-top: 10px;
          margin-bottom: 0;
      }

      .card-chart .card-header .card-category {
          margin-bottom: 5px;
      }

      .card-chart .table {
          margin-bottom: 0;
      }

          .card-chart .table td {
              border-top: none;
              border-bottom: 1px solid #e9ecef;
          }

      .card-chart .card-progress {
          margin-top: 30px;
      }

      .card-chart .chart-area {
          height: 190px;
          width: calc(100% + 30px);
          margin-left: -15px;
          margin-right: -15px;
      }

      .card-chart .card-footer {
          margin-top: 15px;
      }

          .card-chart .card-footer .stats {
              color: #9A9A9A;
          }

      .card-chart .dropdown {
          position: absolute;
          right: 20px;
          top: 20px;
      }

          .card-chart .dropdown .btn {
              margin: 0;
          }

      .card-user .image {
          height: 120px;
      }

      .card-user .author {
          text-align: center;
          text-transform: none;
          margin-top: -77px;
      }

          .card-user .author a + p.description {
              margin-top: -7px;
          }

      .card-user .avatar {
          width: 124px;
          height: 124px;
          border: 1px solid #FFFFFF;
          position: relative;
      }

      .card-user .card-body {
          min-height: 240px;
      }

      .card-user hr {
          margin: 5px 15px;
      }

      .card-user .button-container {
          margin-bottom: 6px;
          text-align: center;
      }

      .card-plain {
          background: transparent;
          -webkit-box-shadow: none;
          box-shadow: none;
      }

          .card-plain .card-header,
          .card-plain .card-footer {
              margin-left: 0;
              margin-right: 0;
              background-color: transparent;
          }

          .card-plain:not(.card-subcategories).card-body {
              padding-left: 0;
              padding-right: 0;
          }

      .card-background {
          background-position: center center;
          background-size: cover;
          text-align: center;
      }

          .card-background .card-body {
              position: relative;
              z-index: 2;
              min-height: 370px;
              max-width: 530px;
              margin: 0 auto;
              padding-top: 60px;
              padding-bottom: 60px;
          }

          .card-background .card-footer {
              position: relative;
              z-index: 2;
          }

          .card-background.card-background-product .card-body {
              max-width: 400px;
          }

              .card-background.card-background-product .card-body .card-title {
                  margin-top: 30px;
              }

          .card-background .stats {
              color: #FFFFFF;
          }

          .card-background .card-footer .stats-link > a {
              color: #FFFFFF;
              line-height: 1.9;
          }

          .card-background .category,
          .card-background .card-description,
          .card-background small {
              color: rgba(255, 255, 255, 0.8);
          }

          .card-background .card-title {
              color: #FFFFFF;
              margin-top: 130px;
          }

          .card-background:not(.card-pricing) .btn {
              margin-bottom: 0;
          }

          .card-background::after {
              position: absolute;
              z-index: 1;
              width: 100%;
              height: 100%;
              display: block;
              left: 0;
              top: 0;
              content: "";
              background-color: rgba(0, 0, 0, 0.63);
              border-radius: 0.25rem;
          }

      .card-collapse .card {
          margin-bottom: 25px;
      }

          .card-collapse .card .card-header {
              position: relative;
              padding: .75rem 1.25rem;
              padding-left: 0;
              padding-right: 0;
          }

              .card-collapse .card .card-header a[data-toggle="collapse"] {
                  display: block;
                  color: #444;
              }

                  .card-collapse .card .card-header a[data-toggle="collapse"] i {
                      float: right;
                      position: relative;
                      color: #f96332;
                      top: 1px;
                  }

              .card-collapse .card .card-header:after {
                  content: "";
                  position: absolute;
                  bottom: 0;
                  left: 0;
                  width: 100%;
                  height: 1px;
                  background-color: #E3E3E3;
              }

          .card-collapse .card .card-body {
              padding-left: .5rem;
              padding-right: .5rem;
          }

      .card-user .card-body {
          min-height: 240px;
      }

      .card .card-body {
          padding: 15px 15px 10px;
      }

      .h5, h5 {
          font-size: 1.57em;
          line-height: 1.4em;
          margin-bottom: 15px;
      }

      h1, h2, h3, h4, h5, h6 {
          font-weight: 400;
      }

      a {
          color: #f96332;
      }

          a:hover, a:focus {
              color: #f96332;
          }

      .description {
          font-size: 1.14em;
      }

      .card-description, .card .footer .stats, .description, .footer-big p {
          color: #9a9a9a;
          font-weight: 300;
      }

      .text-center {
          text-align: center !important;
      }

      p {
          margin-top: 0;
          margin-bottom: 1rem;
      }

      *, :after, :before {
          -webkit-box-sizing: border-box;
          box-sizing: border-box;
      }

      a:hover, a:focus {
          color: #f96332;
      }

      .title {
          font-weight: 700;
      }

          .title.title-up {
              text-transform: uppercase;
          }

              .title.title-up a {
                  color: #2c2c2c;
                  text-decoration: none;
              }

          .title + .category {
              margin-top: -10px;
          }

      .description,
      .card-description,
      .footer-big p,
      .card .footer .stats {
          color: #9A9A9A;
          font-weight: 300;
      }
      /*End profile*/
      /*form*/
      .form-control {
          background-color: transparent;
          border: 1px solid #E3E3E3;
          border-radius: 30px;
          color: #2c2c2c;
          line-height: normal;
          font-size: 0.8571em;
          -webkit-transition: color 0.3s ease-in-out, border-color 0.3s ease-in-out, background-color 0.3s ease-in-out;
          -o-transition: color 0.3s ease-in-out, border-color 0.3s ease-in-out, background-color 0.3s ease-in-out;
          transition: color 0.3s ease-in-out, border-color 0.3s ease-in-out, background-color 0.3s ease-in-out;
          -webkit-box-shadow: none;
          box-shadow: none;
          height: unset;
      }

          .form-control:focus {
              border: 1px solid #f96332;
              -webkit-box-shadow: none;
              box-shadow: none;
              outline: 0 !important;
              color: #2c2c2c;
          }

              .form-control:focus + .input-group-append .input-group-text,
              .form-control:focus ~ .input-group-append .input-group-text,
              .form-control:focus + .input-group-prepend .input-group-text,
              .form-control:focus ~ .input-group-prepend .input-group-text {
                  border: 1px solid #f96332;
                  border-left: none;
                  background-color: transparent;
              }

      .has-success .form-control,
      .has-error .form-control,
      .has-success .form-control:focus,
      .has-error .form-control:focus {
          -webkit-box-shadow: none;
          box-shadow: none;
      }

          .has-danger .form-control.form-control-success, .has-danger .form-control.form-control-danger,
          .has-success .form-control.form-control-success,
          .has-success .form-control.form-control-danger {
              background-image: none;
          }

      .form-control + .form-control-feedback {
          border-radius: 0.25rem;
          font-size: 14px;
          margin-top: -7px;
          position: absolute;
          right: 10px;
          top: 50%;
          vertical-align: middle;
      }

      .open .form-control {
          border-radius: 0.25rem 0.25rem 0 0;
          border-bottom-color: transparent;
      }

      .form-control + .input-group-append .input-group-text,
      .form-control + .input-group-prepend .input-group-text {
          background-color: #FFFFFF;
      }

      .has-success .input-group-append .input-group-text,
      .has-success .input-group-prepend .input-group-text,
      .has-success .form-control {
          border-color: #E3E3E3;
      }

          .has-success .form-control:focus,
          .has-success.input-group-focus .input-group-append .input-group-text,
          .has-success.input-group-focus .input-group-prepend .input-group-text {
              border-color: #1be611;
          }

      .has-danger .form-control,
      .has-danger .input-group-append .input-group-text,
      .has-danger .input-group-prepend .input-group-text,
      .has-danger.input-group-focus .input-group-prepend .input-group-text,
      .has-danger.input-group-focus .input-group-append .input-group-text {
          border-color: #ffcfcf;
          color: #FF3636;
          background-color: rgba(222, 222, 222, 0.1);
      }

          .has-danger .form-control:focus,
          .has-danger .input-group-append .input-group-text:focus,
          .has-danger .input-group-prepend .input-group-text:focus,
          .has-danger.input-group-focus .input-group-prepend .input-group-text:focus,
          .has-danger.input-group-focus .input-group-append .input-group-text:focus {
              background-color: #FFFFFF;
          }

      .has-success:after,
      .has-danger:after {
          font-family: 'Nucleo Outline';
          content: "\ea22";
          display: inline-block;
          position: absolute;
          right: 20px;
          bottom: 10px;
          color: #18ce0f;
          font-size: 11px;
      }

      .has-success.form-control-lg:after,
      .has-danger.form-control-lg:after {
          font-size: 13px;
          top: 24px;
      }

      .has-success.has-label:after,
      .has-danger.has-label:after {
          top: 35px;
      }

      .has-success .form-control + label,
      .has-success.form-check .form-check-label label, .has-success.form-check:after,
      .has-danger .form-control + label,
      .has-danger.form-check .form-check-label label,
      .has-danger.form-check:after {
          display: none !important;
      }

      .has-success.form-check .form-check-label,
      .has-danger.form-check .form-check-label {
          color: #18ce0f;
      }

      .has-danger:after {
          content: "\ea53";
          color: #FF3636;
      }

      .has-danger.form-check .form-check-label {
          color: #FF3636;
      }

      .form-group.no-border.form-control-lg .input-group-append .input-group-text,
      .input-group.no-border.form-control-lg .input-group-append .input-group-text {
          padding: 15px 0 15px 19px;
      }

      .form-group.no-border.form-control-lg .form-control,
      .input-group.no-border.form-control-lg .form-control {
          padding: 15px 19px;
      }

          .form-group.no-border.form-control-lg .form-control + .input-group-prepend .input-group-text,
          .form-group.no-border.form-control-lg .form-control + .input-group-append .input-group-text,
          .input-group.no-border.form-control-lg .form-control + .input-group-prepend .input-group-text,
          .input-group.no-border.form-control-lg .form-control + .input-group-append .input-group-text {
              padding: 15px 19px 15px 0;
          }

      .form-group.form-control-lg .form-control,
      .input-group.form-control-lg .form-control {
          padding: 14px 18px;
      }

          .form-group.form-control-lg .form-control + .input-group-prepend .input-group-text,
          .form-group.form-control-lg .form-control + .input-group-append .input-group-text,
          .input-group.form-control-lg .form-control + .input-group-prepend .input-group-text,
          .input-group.form-control-lg .form-control + .input-group-append .input-group-text {
              padding: 14px 18px 14px 0;
          }

      .form-group.form-control-lg .input-group-prepend .input-group-text,
      .form-group.form-control-lg .input-group-append .input-group-text,
      .input-group.form-control-lg .input-group-prepend .input-group-text,
      .input-group.form-control-lg .input-group-append .input-group-text {
          padding: 14px 0 15px 18px;
      }

          .form-group.form-control-lg .input-group-prepend .input-group-text + .form-control,
          .form-group.form-control-lg .input-group-append .input-group-text + .form-control,
          .input-group.form-control-lg .input-group-prepend .input-group-text + .form-control,
          .input-group.form-control-lg .input-group-append .input-group-text + .form-control {
              padding: 15px 18px 15px 16px;
          }

      .form-group.no-border .form-control,
      .input-group.no-border .form-control {
          padding: 11px 19px;
      }

          .form-group.no-border .form-control + .input-group-prepend .input-group-text,
          .form-group.no-border .form-control + .input-group-append .input-group-text,
          .input-group.no-border .form-control + .input-group-prepend .input-group-text,
          .input-group.no-border .form-control + .input-group-append .input-group-text {
              padding: 11px 19px 11px 0;
          }

      .form-group.no-border .input-group-prepend .input-group-text,
      .form-group.no-border .input-group-append .input-group-text,
      .input-group.no-border .input-group-prepend .input-group-text,
      .input-group.no-border .input-group-append .input-group-text {
          padding: 11px 0 11px 19px;
      }

      .form-group .form-control,
      .input-group .form-control {
          padding: 10px 18px 10px 18px;
      }

          .form-group .form-control + .input-group-prepend .input-group-text,
          .form-group .form-control + .input-group-append .input-group-text,
          .input-group .form-control + .input-group-prepend .input-group-text,
          .input-group .form-control + .input-group-append .input-group-text {
              padding: 10px 18px 10px 0;
          }

      .form-group .input-group-prepend .input-group-text,
      .form-group .input-group-append .input-group-text,
      .input-group .input-group-prepend .input-group-text,
      .input-group .input-group-append .input-group-text {
          padding: 10px 0 10px 18px;
      }

          .form-group .input-group-prepend .input-group-text + .form-control,
          .form-group .input-group-prepend .input-group-text ~ .form-control,
          .form-group .input-group-append .input-group-text + .form-control,
          .form-group .input-group-append .input-group-text ~ .form-control,
          .input-group .input-group-prepend .input-group-text + .form-control,
          .input-group .input-group-prepend .input-group-text ~ .form-control,
          .input-group .input-group-append .input-group-text + .form-control,
          .input-group .input-group-append .input-group-text ~ .form-control {
              padding: 10px 19px 11px 16px;
          }

      .form-group.no-border .form-control,
      .form-group.no-border .form-control + .input-group-prepend .input-group-text,
      .form-group.no-border .form-control + .input-group-append .input-group-text,
      .input-group.no-border .form-control,
      .input-group.no-border .form-control + .input-group-prepend .input-group-text,
      .input-group.no-border .form-control + .input-group-append .input-group-text {
          background-color: rgba(222, 222, 222, 0.3);
          border: medium none;
      }

          .form-group.no-border .form-control:focus, .form-group.no-border .form-control:active,
          .form-group.no-border .form-control:active,
          .form-group.no-border .form-control + .input-group-prepend .input-group-text:focus,
          .form-group.no-border .form-control + .input-group-prepend .input-group-text:active,
          .form-group.no-border .form-control + .input-group-prepend .input-group-text:active,
          .form-group.no-border .form-control + .input-group-append .input-group-text:focus,
          .form-group.no-border .form-control + .input-group-append .input-group-text:active,
          .form-group.no-border .form-control + .input-group-append .input-group-text:active,
          .input-group.no-border .form-control:focus,
          .input-group.no-border .form-control:active,
          .input-group.no-border .form-control:active,
          .input-group.no-border .form-control + .input-group-prepend .input-group-text:focus,
          .input-group.no-border .form-control + .input-group-prepend .input-group-text:active,
          .input-group.no-border .form-control + .input-group-prepend .input-group-text:active,
          .input-group.no-border .form-control + .input-group-append .input-group-text:focus,
          .input-group.no-border .form-control + .input-group-append .input-group-text:active,
          .input-group.no-border .form-control + .input-group-append .input-group-text:active {
              border: medium none;
              background-color: rgba(222, 222, 222, 0.5);
          }

              .form-group.no-border .form-control:focus + .input-group-prepend .input-group-text,
              .form-group.no-border .form-control:focus + .input-group-append .input-group-text,
              .input-group.no-border .form-control:focus + .input-group-prepend .input-group-text,
              .input-group.no-border .form-control:focus + .input-group-append .input-group-text {
                  background-color: rgba(222, 222, 222, 0.5);
              }

      .form-group.no-border .input-group-prepend .input-group-text,
      .form-group.no-border .input-group-append .input-group-text,
      .input-group.no-border .input-group-prepend .input-group-text,
      .input-group.no-border .input-group-append .input-group-text {
          background-color: rgba(222, 222, 222, 0.3);
          border: none;
      }

      .has-error .form-control-feedback, .has-error .control-label {
          color: #FF3636;
      }

      .has-success .form-control-feedback, .has-success .control-label {
          color: #18ce0f;
      }
      /*End*/
      /*Header*/
      .card [class*=card-header-] {
          margin: 0 15px;
          padding: 0;
          position: relative;
      }

      .card .card-header {
          z-index: 3 !important;
      }

      card .card-header {
          border-bottom: none;
          background: 0 0;
      }

      .card .card-header .card-title {
          margin-top: -5px;
      }

      .card .card-header .title, .card [class*=card-header-], .card [class*=card-header-] .card-title, .card [class*=card-header-] .card-title a, .card [class*=card-header-] .icon i, .card[class*=bg-], .card[class*=bg-] .card-title, .card[class*=bg-] .card-title a, .card[class*=bg-] .icon i {
          color: #000;
      }

      .card .card-header:not([data-background-color]) {
          background-color: transparent;
      }

      .card .card-header {
          padding: 15px 15px 0;
          border: 0;
      }

      .card [class*=card-header-] .card-icon, .card [class*=card-header-] .card-text {
          border-radius: 3px;
          background-color: #999;
          padding: 15px;
          margin-top: -20px;
          margin-left: 15px;
          float: right;
      }

      .card-header:first-child {
          border-radius: calc(.25rem - 1px) calc(.25rem - 1px) 0 0;
      }
      /*End header*/
      .card, .picture-src {
          width: 100%;
      }

          .card .card-body {
              padding: 15px 15px 10px;
          }

      @media screen and (max-width: 768px) {
          [class*="pr-"] {
              padding-right: 15px !important;
          }

          [class*="pl-"] {
              padding-left: 15px !important;
          }

          [class*="px-"] {
              padding-left: 15px !important;
              padding-right: 15px !important;
          }
      }

      .position-relative {
          position: relative !important;
      }

      .form-group, .input-group el-select {
          margin-bottom: 10px;
          position: relative;
      }
      /*card lables*/
      card label {
          font-size: .8571em;
          margin-bottom: 5px;
          color: #9a9a9a;
      }

      .card-chart .card-header .card-title {
          margin-top: 10px;
          margin-bottom: 0;
          margin-right: 20px;
      }
      /*card lables*/
  </style>